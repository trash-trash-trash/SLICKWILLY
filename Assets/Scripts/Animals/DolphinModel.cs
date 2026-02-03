using System;
using UnityEngine;

public class DolphinModel : MonoBehaviour
{
    public Rigidbody rb;

    public float speed;

    public float maxspeed;
    public Vector3 destination;
    
    void Update()
    {
        MoveTowards(destination);
    }
    
    void MoveTowards(Vector3 targetPos)
    {
        Vector3 direction = targetPos - transform.position;
        direction.y = 0f;
        rb.AddForce(direction * speed, ForceMode.Acceleration);

        if (rb.linearVelocity.magnitude > maxspeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxspeed;
        }
    }
}
