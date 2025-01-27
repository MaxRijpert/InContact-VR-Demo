using UnityEngine;

public class CarAutoStart : MonoBehaviour
{
    public Rigidbody carRigidbody; // Reference to the car's Rigidbody
    public WheelCollider[] wheelColliders; // References to the car's WheelColliders
    public float initialBrakeTorque = 2000f; // Brake torque to apply initially

    void Start()
    {
        // Reset Rigidbody velocity and position
        if (carRigidbody != null)
        {
            carRigidbody.linearVelocity = Vector3.zero;
            carRigidbody.angularVelocity = Vector3.zero;
            Debug.Log("[CarAutoStart] Car Rigidbody reset.");
        }
        else
        {
            Debug.LogWarning("[CarAutoStart] No Rigidbody assigned.");
        }

        // Reset all WheelColliders
        if (wheelColliders != null && wheelColliders.Length > 0)
        {
            foreach (WheelCollider wheel in wheelColliders)
            {
                wheel.motorTorque = 0;
                wheel.brakeTorque = initialBrakeTorque; // Ensure the car stays still initially
            }
            Debug.Log("[CarAutoStart] WheelColliders reset.");
        }
        else
        {
            Debug.LogWarning("[CarAutoStart] No WheelColliders assigned.");
        }
    }
}
