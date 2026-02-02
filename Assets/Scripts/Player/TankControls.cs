using System.Collections.Generic;
using UnityEngine;

public class TankControls : MonoBehaviour
{
    public PlayerInputHandler inputHandler;
    public Rigidbody rb;

    [Header("Movement Settings")]
    public float accelerationForce = 30f;
    public float reverseForce = 20f;
    public float turnTorque = 12f;

    public float topSpeed = 12f;
    public float topTurnSpeed = 2.5f;

    private Vector2 moveInput;

    [Header("Surface Modifiers")]
    public float oilSpeedMultiplier = 0.4f;
    public float currentSpeedMultiplier = 1f;

    [Header("Surface Detection")]
    public LayerMask oceanTileLayer;  
    public Vector3 overlapSize = new Vector3(1f, 1f, 1f); 
    public Vector3 overlapOffset = new Vector3(0f, -0.5f, 0f); 

    void OnEnable()
    {
        inputHandler.AnnounceMovement += OnMoveInput;
    }

    private void OnMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    private void FixedUpdate()
    {
        CheckSurface();

        float forwardInput = moveInput.y;
        float turnInput = moveInput.x;

        if (forwardInput > 0f)
            Accelerate(forwardInput);
        if (forwardInput < 0f)
            Reverse(-forwardInput);
        if (Mathf.Abs(turnInput) > 0.01f)
            Spin(turnInput);
    }

    private void Accelerate(float amount)
    {
        if (IsAtTopSpeed()) return;

        Vector3 forward = rb.rotation * Vector3.forward;
        rb.AddForce(forward * (accelerationForce * currentSpeedMultiplier * amount), ForceMode.Force);
    }

    private void Reverse(float amount)
    {
        if (IsAtTopSpeed()) return;

        Vector3 forward = rb.rotation * Vector3.forward;
        rb.AddForce(-forward * (reverseForce * currentSpeedMultiplier * amount), ForceMode.Force);
    }

    private void Spin(float amount)
    {
        float currentTurnSpeed = rb.angularVelocity.y;

        if (Mathf.Abs(currentTurnSpeed) >= topTurnSpeed &&
            Mathf.Sign(currentTurnSpeed) == Mathf.Sign(amount))
            return;

        rb.AddTorque(Vector3.up * (turnTorque * amount), ForceMode.Force);

        Vector3 angVel = rb.angularVelocity;
        angVel.y = Mathf.Clamp(angVel.y, -topTurnSpeed, topTurnSpeed);
        rb.angularVelocity = angVel;
    }

    private bool IsAtTopSpeed()
    {
        float currentTopSpeed = topSpeed * currentSpeedMultiplier;
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        return flatVel.magnitude >= currentTopSpeed;
    }

    private void CheckSurface()
    {
        currentSpeedMultiplier = 1f;

        Vector3 center = transform.position + overlapOffset;

        Collider[] hits = Physics.OverlapBox(center, overlapSize * 0.5f, Quaternion.identity, oceanTileLayer);

        foreach (Collider hit in hits)
        {
            OceanTile tile = hit.GetComponent<OceanTile>();
            if (tile != null && tile.OceanType == OceanType.Oil)
            {
                currentSpeedMultiplier = oilSpeedMultiplier;
                break; //only need to find one oil tile to lower speed
            }
        }
    }

    void OnDisable()
    {
        inputHandler.AnnounceMovement -= OnMoveInput;
    }
}
