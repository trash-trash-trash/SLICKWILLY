using System;
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

    [Header("Lateral Friction")]
    public float lateralFriction = 10f; 

    public bool canControl = false;
    
    public LayerMask playerLayer;
    public LayerMask cleanerLayer;

    public bool isMoving;
    public bool isOily;
    public Action<bool> MovingEvent;
    public Action<bool> AnnounceCameraEvent;

    void OnEnable()
    {
        Physics.IgnoreLayerCollision(playerLayer, cleanerLayer);
        inputHandler.AnnounceMovement += OnMoveInput;
        inputHandler.AnnounceSpaceBar += CameraToggle;
    }

    public void FlipControlOnOff(bool input)
    {
        canControl = input;
    }

    private void CameraToggle(bool obj)
    {
        AnnounceCameraEvent?.Invoke(obj);
    }

    private void OnMoveInput(Vector2 input)
    {
        if (!canControl)
            return;
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

        ApplyLateralFriction(); // <-- new
        
        bool currentlyMoving = rb.linearVelocity.sqrMagnitude > 0.01f;
            
        if (currentlyMoving != isMoving)
        {
            isMoving = currentlyMoving;
            MovingEvent?.Invoke(isMoving);
        }
    }

    private void Accelerate(float amount)
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        float currentTopSpeed = topSpeed * currentSpeedMultiplier;
        float speedFactor = Mathf.Clamp01(1f - flatVel.magnitude / currentTopSpeed);

        Vector3 forward = rb.rotation * Vector3.forward;
        rb.AddForce(forward * (accelerationForce * currentSpeedMultiplier * amount * speedFactor), ForceMode.Force);
    }

    private void Reverse(float amount)
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        float currentTopSpeed = topSpeed * currentSpeedMultiplier;
        float speedFactor = Mathf.Clamp01(1f - flatVel.magnitude / currentTopSpeed);

        Vector3 forward = rb.rotation * Vector3.forward;
        rb.AddForce(-forward * (reverseForce * currentSpeedMultiplier * amount * speedFactor), ForceMode.Force);
    }

    private void Spin(float amount)
    {
        rb.AddTorque(Vector3.up * (turnTorque * amount), ForceMode.Force);

        Vector3 angVel = rb.angularVelocity;
        angVel.y = Mathf.Clamp(angVel.y, -topTurnSpeed, topTurnSpeed);
        rb.angularVelocity = angVel;
    }

    private void CheckSurface()
    {
        isOily = false;
        currentSpeedMultiplier = 1f;

        Vector3 center = transform.position + overlapOffset;
        Collider[] hits = Physics.OverlapBox(center, overlapSize * 0.5f, Quaternion.identity, oceanTileLayer);

        foreach (Collider hit in hits)
        {
            OceanTile tile = hit.GetComponent<OceanTile>();
            if (tile != null && tile.oceanType == OceanType.Oil)
            {
                currentSpeedMultiplier = oilSpeedMultiplier;
                isOily = true;
                break;
            }
        }
    }

    private void ApplyLateralFriction()
    {
        Vector3 localVel = transform.InverseTransformDirection(rb.linearVelocity);

        Vector3 lateralForce = new Vector3(-localVel.x * lateralFriction, 0f, 0f);

        rb.AddRelativeForce(lateralForce, ForceMode.Force);
    }

    void OnDisable()
    {
        inputHandler.AnnounceMovement -= OnMoveInput;
        inputHandler.AnnounceSpaceBar -= CameraToggle;
    }
}
