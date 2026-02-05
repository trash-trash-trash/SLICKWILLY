using System;
using UnityEngine;

public class DolphinModel : MonoBehaviour
{
    public Rigidbody rb;

    public float speed;
    public float maxspeed;

    public float turnSpeed = 360f;
    public float turnAngleRange = 45f;
        
    private bool isTurning = false;
    private Quaternion targetRotation;

    public LayerMask oceanTileLayer;
    public LayerMask invisibleWallLayer;
    public LayerMask animalLayer;
    public LayerMask playerLayer;
    
    public OilComponent oilComponent;
    
    [Header("Wall Hit Response")]
    public float wallBounceForce = 2f;
    public float wallPauseTime = 0.15f;
    private bool isStunned = false;
    
    public float wallRepelForce = 100f; // force applied while inside wall

    void OnEnable()
    {
        Physics.IgnoreLayerCollision(animalLayer, animalLayer);
        Physics.IgnoreLayerCollision(animalLayer, playerLayer);
        oilComponent.Dirty();
    }

    void Update()
    {
        if (isStunned)
            return;
        
        if (!isTurning)
            MoveForwards();
        else
            TurnTowardsTarget();
    }

    void MoveForwards()
    {
        rb.AddRelativeForce(Vector3.forward * speed, ForceMode.Acceleration);

        if (rb.linearVelocity.magnitude > maxspeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxspeed;
        }
    }

    void TurnTowardsTarget()
    {
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            turnSpeed * Time.deltaTime
        );

        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.5f)
        {
            isTurning = false;
        }
    }

    //when dolphin hits barrier, spin 180 degrees to turn around, then random between -turnAngleRange
    void StartTurnAround(Vector3 wallNormal)
    {
        // Push slightly away from wall
        rb.AddForce(-wallNormal * wallBounceForce, ForceMode.VelocityChange);

        // Begin turn
        float randomOffset = UnityEngine.Random.Range(-turnAngleRange, turnAngleRange);
        float turnAmount = 180f + randomOffset;

        targetRotation = Quaternion.Euler(
            0f,
            transform.eulerAngles.y + turnAmount,
            0f
        );

        isTurning = true;

        StartCoroutine(WallPause());
    }

    System.Collections.IEnumerator WallPause()
    {
        isStunned = true;
        yield return new WaitForSeconds(wallPauseTime);
        isStunned = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & invisibleWallLayer.value) != 0)
        { 
            Vector3 wallNormal = (transform.position - other.ClosestPoint(transform.position)).normalized;
            StartTurnAround(wallNormal);
        }
        else if(((1 << other.gameObject.layer) & oceanTileLayer.value) != 0)
        {
            Debug.Log("hit ocean tile");
            if (oilComponent.IsOily)
            {
                OilComponent oil = other.gameObject.GetComponent<OilComponent>();
                oil.Dirty();
            }
            else
            {
                OilComponent oil = other.gameObject.GetComponent<OilComponent>();
                oil.Clean();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (((1 << other.gameObject.layer) & invisibleWallLayer.value) != 0)
        {
            // Push away from wall
            Vector3 repelDir = (transform.position - other.ClosestPoint(transform.position)).normalized;
            rb.AddForce(repelDir * wallRepelForce, ForceMode.Acceleration);
        }
    }
}