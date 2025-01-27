using UnityEngine;
using UnityEngine.InputSystem;

public class CarControl3 : MonoBehaviour
{
    public float motorTorque = 2000;
    public float brakeTorque = 2000;
    public float maxSpeed = 20;
    public float steeringRange = 30;
    public float steeringRangeAtMaxSpeed = 10;
    public float centreOfGravityOffset = -1f;
    public float accelerationTime = 2f;
    public float brakeDelay = 1f;
    public float brakingTime = 2f;

    public float accelerationStiffness = 1f;
    public float brakingStiffness = 0.5f;
    public float accelerationStiffnessDelay = 0.5f;
    public float brakingStiffnessDelay = 1f;

    public InputAction toggleVisibilityAction;
    public GameObject[] devicesToToggle;

    // Audio
    public AudioSource engineAudioSource;  // Assign this in the Inspector
    public float minPitch = 0.5f;
    public float maxPitch = 2.0f;

    private WheelControl[] wheels;
    private Rigidbody rigidBody;
    private bool devicesVisible = true;

    private enum CarState { Accelerating, Delaying, Braking, Stopped }
    private CarState currentState;
    private float stateTimer = 0f;
    private bool accelerationStiffnessApplied = false;
    private bool brakingStiffnessApplied = false;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.centerOfMass += Vector3.up * centreOfGravityOffset;
        wheels = GetComponentsInChildren<WheelControl>();

        if (wheels == null || wheels.Length == 0)
        {
            Debug.LogWarning("No WheelControl components found as children of CarControl2.");
        }

        currentState = CarState.Stopped;
        toggleVisibilityAction.Enable();
        toggleVisibilityAction.performed += ToggleDeviceVisibility;

        // Initialize audio
        if (engineAudioSource != null)
        {
            engineAudioSource.loop = true;
            engineAudioSource.Play(); // Starts playing immediately but won't be heard until volume is set in Update
        }
    }

    void OnDestroy()
    {
        toggleVisibilityAction.performed -= ToggleDeviceVisibility;
    }

    void Update()
    {
        stateTimer += Time.deltaTime;

        // Update car state
        switch (currentState)
        {
            case CarState.Accelerating:
                ApplyAcceleration();
                if (stateTimer >= accelerationTime)
                {
                    currentState = CarState.Delaying;
                    stateTimer = 0f;
                    accelerationStiffnessApplied = false;
                }
                break;

            case CarState.Delaying:
                ApplyCoasting();
                if (stateTimer >= brakeDelay)
                {
                    currentState = CarState.Braking;
                    stateTimer = 0f;
                    brakingStiffnessApplied = false;
                }
                break;

            case CarState.Braking:
                ApplyBraking();
                if (stateTimer >= brakingTime)
                {
                    currentState = CarState.Stopped;
                    StopCar();
                }
                break;

            case CarState.Stopped:
                StopCar();
                break;
        }

        // Adjust audio pitch based on speed
        if (engineAudioSource != null)
        {
            float speed = rigidBody.linearVelocity.magnitude;
            float pitch = Mathf.Lerp(minPitch, maxPitch, speed / maxSpeed);
            engineAudioSource.pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }
    }

    private void ApplyAcceleration()
    {
        if (wheels == null || wheels.Length == 0) return;

        float accelerationProgress = Mathf.Clamp01(stateTimer / accelerationTime);
        float currentMotorTorque = motorTorque * accelerationProgress;

        foreach (var wheel in wheels)
        {
            if (wheel.steerable) wheel.WheelCollider.steerAngle = 0;
            if (wheel.motorized) wheel.WheelCollider.motorTorque = currentMotorTorque;
            wheel.WheelCollider.brakeTorque = 0;
        }

        if (!accelerationStiffnessApplied && stateTimer >= accelerationStiffnessDelay)
        {
            SetWheelStiffness(accelerationStiffness);
            accelerationStiffnessApplied = true;
        }
    }

    private void ApplyCoasting()
    {
        if (wheels == null || wheels.Length == 0) return;

        foreach (var wheel in wheels)
        {
            if (wheel.motorized) wheel.WheelCollider.motorTorque = 0;
            wheel.WheelCollider.brakeTorque = 0;
        }
    }

    private void ApplyBraking()
    {
        if (wheels == null || wheels.Length == 0) return;

        float brakingProgress = Mathf.Clamp01(stateTimer / brakingTime);
        float currentBrakeTorque = brakeTorque * (1 - brakingProgress);

        foreach (var wheel in wheels)
        {
            wheel.WheelCollider.motorTorque = 0;
            wheel.WheelCollider.brakeTorque = currentBrakeTorque;
        }

        if (!brakingStiffnessApplied && stateTimer >= brakingStiffnessDelay)
        {
            SetWheelStiffness(brakingStiffness);
            brakingStiffnessApplied = true;
        }
    }

    private void StopCar()
    {
        if (wheels == null || wheels.Length == 0) return;

        foreach (var wheel in wheels)
        {
            wheel.WheelCollider.motorTorque = 0;
            wheel.WheelCollider.brakeTorque = brakeTorque;
        }
    }

    public void ResetControl()
    {
        if (wheels == null || wheels.Length == 0) return;

        foreach (var wheel in wheels)
        {
            wheel.WheelCollider.motorTorque = 0;
            wheel.WheelCollider.brakeTorque = brakeTorque;
        }

        currentState = CarState.Accelerating;
        stateTimer = 0f;
        accelerationStiffnessApplied = false;
        brakingStiffnessApplied = false;
    }

    private void ToggleDeviceVisibility(InputAction.CallbackContext context)
    {
        devicesVisible = !devicesVisible;
        foreach (var device in devicesToToggle)
        {
            device.SetActive(devicesVisible);
        }
    }

    private void SetWheelStiffness(float stiffness)
    {
        if (wheels == null || wheels.Length == 0) return;

        foreach (var wheel in wheels)
        {
            var forwardFriction = wheel.WheelCollider.forwardFriction;
            forwardFriction.stiffness = stiffness;
            wheel.WheelCollider.forwardFriction = forwardFriction;

            var sidewaysFriction = wheel.WheelCollider.sidewaysFriction;
            sidewaysFriction.stiffness = stiffness;
            wheel.WheelCollider.sidewaysFriction = sidewaysFriction;
        }
    }

}
