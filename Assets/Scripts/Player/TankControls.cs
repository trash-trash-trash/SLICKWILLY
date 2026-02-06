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
    
    public AnimationCurve turnSpeedCurve;
    public AnimationCurve lateralGripSpeedCurve;

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
   
    [SerializeField]
    private bool inOil;

    [SerializeField]
    private GameObject view;
    
    
    float forwardInput;
    float turnInput;

    [SerializeField]
    private float speedCurveScale = 0.01f;
    
    [Header("Debug")]
    public float   currentTopSpeed;
    public float curvePosition;

    
    void OnEnable()
    {
        Physics.IgnoreLayerCollision(3, 10);
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

    private void Update()
    {
	    // Wavey effect
	    // float vel          = rb.linearVelocity.magnitude;

		float turnSpeedCurveScale = 0.1f;
		float f                   = (turnSpeedCurve.Evaluate(rb.linearVelocity.magnitude * turnSpeedCurveScale));

		float velZ = transform.InverseTransformDirection(rb.linearVelocity).z;
		velZ = Mathf.Clamp(velZ, -10f, 30f);
	    
	    float targetZAngle = -turnInput * 40f;
	    targetZAngle = Mathf.Clamp(targetZAngle, -35f, 35f);
	    Quaternion targetRotation = Quaternion.Euler(-velZ, 0, targetZAngle * f);

	    view.transform.localRotation = Quaternion.Lerp(view.transform.localRotation, targetRotation, 0.1f);
	    
	    // float linearVelocityMagnitudeX = Mathf.PerlinNoise1D(Time.time*vel)*10f*(vel);
	    // float linearVelocityMagnitudeZ = Mathf.PerlinNoise1D(Time.time*1.2f*vel)*10f*(vel);
	    // view.transform.rotation = transform.rotation * Quaternion.Euler((linearVelocityMagnitudeX*2f)-1f, 0f, (linearVelocityMagnitudeZ*2f)-1f);
	    //
	    // view.transform.rotation = transform.rotation;
	    //
	    // float zAngle = Mathf.Lerp(view.transform.localRotation.eulerAngles.z, -turnInput*40f, 15.1f*Time.deltaTime);
	    // view.transform.localRotation = Quaternion.Euler(-vel, 0, zAngle);
	    // view.transform.Rotate(-vel,0,zAngle, Space.Self);
    }

    private void FixedUpdate()
    {
        CheckSurface();

        if (inOil)
        {
	        // if(rb.GetRelativePointVelocity(Vector3.zero).z > 0)
		        // rb.AddRelativeForce(0,0,-1200f);
	        rb.linearDamping = 3;
        }
        else
        {
	        rb.linearDamping = 1;
        }
        
        forwardInput = moveInput.y;
        turnInput = moveInput.x;

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
        currentTopSpeed = topSpeed * currentSpeedMultiplier;
        float   speedFactor = Mathf.Clamp01(1f - flatVel.magnitude / currentTopSpeed);

        Vector3 forward = rb.rotation * Vector3.forward;
        rb.AddForce(forward * (accelerationForce * currentSpeedMultiplier * amount * speedFactor), ForceMode.Force);
    }

    private void Reverse(float amount)
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        float currentTopSpeed = topSpeed * currentSpeedMultiplier;
        float speedFactor = Mathf.Clamp01(1f - flatVel.magnitude / currentTopSpeed);
        
        Vector3 forward = rb.rotation * Vector3.forward;
        rb.AddForce(-forward * (reverseForce * currentSpeedMultiplier * amount * speedFactor) / 4f, ForceMode.Force);
    }

    private void Spin(float amount)
    {
	    curvePosition = rb.linearVelocity.magnitude * speedCurveScale;
	    float curveValue = turnSpeedCurve.Evaluate(curvePosition);
	    float torque     = (turnTorque * amount) * curveValue;
	    if (inOil)
	    {
		    rb.AddTorque(Vector3.up * torque * 1f, ForceMode.Force);
	    }
	    else
	    {
		    rb.AddTorque(Vector3.up * torque * 2f, ForceMode.Force);
	    }

        // Vector3 angVel = rb.angularVelocity;
        // angVel.y = Mathf.Clamp(angVel.y, -topTurnSpeed, topTurnSpeed);
        // rb.angularVelocity = angVel;
    }

    private void CheckSurface()
    {
        currentSpeedMultiplier = 1f;
        
        inOil = false;

        Vector3 center = transform.position + overlapOffset;
        Collider[] hits = Physics.OverlapBox(center, overlapSize * 0.5f, Quaternion.identity, oceanTileLayer);

        foreach (Collider hit in hits)
        {
            OceanTile tile = hit.GetComponent<OceanTile>();
            if (tile != null && tile.oceanType == OceanType.Oil)
            {
                currentSpeedMultiplier = oilSpeedMultiplier;
                inOil                  = true;
                isOily                 = true;
                break;
            }
            else if(tile != null && tile.oceanType == OceanType.Water)
            {
	            inOil  = false;
                isOily = false;
            }
        }
    }

    private void ApplyLateralFriction()
    {
        Vector3 localVel = transform.InverseTransformDirection(rb.linearVelocity);


        Vector3 lateralForce;
        if (inOil)
        {
	        lateralForce = new Vector3(-localVel.x * lateralFriction * 13f, 0f, 0f);
        }
        else
        {
	        float forwardBoostFromDrift = 25f;
	        forwardBoostFromDrift = Mathf.Clamp(forwardBoostFromDrift, 0f, 100f);

	        float curvedLateralFriction = lateralGripSpeedCurve.Evaluate(curvePosition);
	        
	        lateralForce = new Vector3(-localVel.x * curvedLateralFriction, 0f, Mathf.Abs(localVel.x*forwardBoostFromDrift));
        }

        rb.AddRelativeForce(lateralForce, ForceMode.Force);
    }

    void OnDisable()
    {
        inputHandler.AnnounceMovement -= OnMoveInput;
        inputHandler.AnnounceSpaceBar -= CameraToggle;
    }
}
