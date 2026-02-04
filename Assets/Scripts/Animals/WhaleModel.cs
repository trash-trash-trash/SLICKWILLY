using UnityEngine;

public class WhaleModel : MonoBehaviour
{
    public Rigidbody rb;

    public float speed;

    public float maxspeed;
    
    void Update()
    {
        MoveForwards();
    }
    
    void MoveForwards()
    {
        
        rb.AddRelativeForce(Vector3.forward * speed, ForceMode.Acceleration);

        if (rb.linearVelocity.magnitude > maxspeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxspeed;
        }
    }
}
