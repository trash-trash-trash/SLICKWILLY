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
        if (IsAtTopSpeed())
            return;

        Vector3 forward = rb.rotation * Vector3.forward;
        rb.AddForce(forward * (accelerationForce * amount), ForceMode.Force);
    }

    private void Reverse(float amount)
    {
        if (IsAtTopSpeed())
            return;

        Vector3 forward = rb.rotation * Vector3.forward;
        rb.AddForce(-forward * (reverseForce * amount), ForceMode.Force);
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
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        return flatVel.magnitude >= topSpeed;
    }
    
    void OnDisable()
    {
        inputHandler.AnnounceMovement -= OnMoveInput;
    }
}
