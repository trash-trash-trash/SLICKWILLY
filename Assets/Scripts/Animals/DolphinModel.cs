using System;
using UnityEngine;

public class DolphinModel : MonoBehaviour
{
    public Rigidbody rb;

    public float speed;
    public float maxspeed;

    [Header("Turning")]
    public float turnSpeed = 360f; 
    
    private bool isTurning = false;
    private Quaternion targetRotation;

    public LayerMask oceanTileLayer;
    public LayerMask invisibleWallLayer;

    public OilComponent oilComponent;

    void Update()
    {
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

    //when dolphin hits barrier, spin 180 degrees to turn around, then random between -75 and 75
    void StartTurnAround()
    {
        float randomOffset = UnityEngine.Random.Range(-75f, 75f);
        float turnAmount = 180f + randomOffset;

        targetRotation = Quaternion.Euler(0f, transform.eulerAngles.y + turnAmount, 0f);
        isTurning = true;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & invisibleWallLayer.value) != 0)
        {
            StartTurnAround();
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

}