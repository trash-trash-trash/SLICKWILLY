using UnityEngine;

public class DolphinModel : MonoBehaviour
{
    public OilComponent oilComponent;

    [Header("Physics")] public Rigidbody rb;
    public float forwardForce = 15f;
    public float maxSpeed = 8f;
    public float turnSpeed = 4f;
    public float escapeForceBoost = 20f;

    [Header("Wobble")] public float wobbleStrength = 20f;
    public float wobbleFrequency = 0.5f;
    private float wobbleSeed;

    [Header("Wall Avoidance")] public LayerMask invisibleWallLayer;
    public float wallDetectRadius = 10f;
    public float escapeRayDistance = 25f;
    public int escapeRayAttempts = 25;

    public float wallRepelForce = 30f;
    public float wallRepelFalloff = 1f;

    private bool escaping = false;
    private Vector3 escapeDirection;

    public LayerMask oceanTileLayer;

    private Collider[] wallHits = new Collider[8];

    public bool canMove = true;

    void Awake()
    {
        oilComponent = GetComponent<OilComponent>();
        wobbleSeed = Random.Range(0f, 1000f);
        Physics.IgnoreLayerCollision(9,9);
    }

    void FixedUpdate()
    {
        if(canMove)
        {
            LimitSpeed();

            if (DetectWall())
            {
                HandleEscape();
            }
            else
            {
                SwimForward();
            }
        }
    }

    public void FlipCanMove(bool input)
    {
        canMove = input;
        
        oilComponent.FlipCanBeCleaned(input);
    }


    void SwimForward()
    {
        escaping = false;

        rb.AddForce(transform.forward * forwardForce, ForceMode.Acceleration);

        float noise = Mathf.PerlinNoise(Time.time * wobbleFrequency, wobbleSeed);
        float steer = (noise - 0.5f) * 2f * wobbleStrength;

        rb.AddTorque(Vector3.up * steer, ForceMode.Acceleration);
    }

    void LimitSpeed()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (flatVel.magnitude > maxSpeed)
        {
            rb.linearVelocity = flatVel.normalized * maxSpeed + Vector3.up * rb.linearVelocity.y;
        }
    }


    bool DetectWall()
    {
        int count = Physics.OverlapSphereNonAlloc(
            transform.position,
            wallDetectRadius,
            wallHits,
            invisibleWallLayer
        );

        if (count > 0)
        {
            ApplyWallRepulsion(count);
            return true;
        }

        return false;
    }


    void ApplyWallRepulsion(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Collider col = wallHits[i];
            if (col == null) continue;

            Vector3 closest = col.ClosestPoint(transform.position);
            Vector3 away = transform.position - closest;

            float dist = away.magnitude;
            if (dist < 0.001f) continue;

            float strength = 1f - Mathf.Clamp01(dist / wallDetectRadius);
            Vector3 force = away.normalized * wallRepelForce * strength * wallRepelFalloff;

            rb.AddForce(force, ForceMode.Acceleration);
        }
    }


    void HandleEscape()
    {
        if (!escaping)
        {
            escaping = true;
            escapeDirection = FindEscapeDirection();
            rb.AddForce(escapeDirection * escapeForceBoost, ForceMode.VelocityChange);
        }

        Quaternion targetRot = Quaternion.LookRotation(escapeDirection, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, turnSpeed * Time.fixedDeltaTime);
    }

    Vector3 FindEscapeDirection()
    {
        Vector3 backward = -transform.forward;

        for (int i = 0; i < escapeRayAttempts; i++)
        {
            float angle = Random.Range(-90f, 90f);
            Vector3 dir = Quaternion.Euler(0f, angle, 0f) * backward;

            if (!Physics.Raycast(transform.position, dir, escapeRayDistance, invisibleWallLayer))
            {
                return dir.normalized;
            }
        }

        //worst case: full reverse
        return backward.normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & oceanTileLayer.value) != 0)
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, wallDetectRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 3f);
    }
}