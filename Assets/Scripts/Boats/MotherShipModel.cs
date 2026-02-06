using System;
using System.Collections.Generic;
using UnityEngine;

public class MotherShipModel : MonoBehaviour
{
    public Rigidbody rb;

    public float speed;

    public float maxspeed;
    public float rotationSpeed;
    public float arrivalRadius;

    public List<GameObject> destinations;

    private int currentDestinationIndex;

    public bool stopShip;
    
    private bool isMoving;
    public Action<bool> MovingEvent;

    void Start()
    {
        currentDestinationIndex = 0;
    }

    void Update()
    {
        if (destinations == null || destinations.Count == 0)
        {
            return;
        }

        Vector3 targetPos = destinations[currentDestinationIndex].transform.position;

        if(!stopShip)
        {
            MoveTowards(targetPos);
            LookAt(targetPos);
            DestinationCheck(targetPos);
        }

        bool currentlyMoving = rb.linearVelocity.sqrMagnitude > 0.01f;
            
        if (currentlyMoving != isMoving)
        {
            isMoving = currentlyMoving;
            MovingEvent?.Invoke(isMoving);
        }
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
    
    void LookAt(Vector3 targetPos)
    {
        Vector3 lookDir = targetPos - transform.position;
        lookDir.y = 0f;

        if (lookDir.sqrMagnitude < 0.01f)
        {
            return;
        }
        
        Quaternion targetRotation = Quaternion.LookRotation(lookDir);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation,targetRotation,Time.deltaTime * rotationSpeed));
    }
    
    void DestinationCheck(Vector3 targetPos)
    {
        if (Vector3.Distance(transform.position, targetPos) <= arrivalRadius)
        {
            currentDestinationIndex++;

            if (currentDestinationIndex >= destinations.Count)
            {
                currentDestinationIndex = 0;
            }
        }
    }
}
