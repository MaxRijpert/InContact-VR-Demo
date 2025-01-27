using UnityEngine;
using UnityEngine.InputSystem;

public class EyeContactDevice : MonoBehaviour
{
    public Transform playerCamera; // Assign the player's camera here
    public float detectionRange = 10f; // Range within which the device starts looking at the player
    public Transform unitCover; // Reference to Unit_Cover_Parent for horizontal rotation
    public Transform unitSphere; // Reference to Unit_Sphere for vertical rotation
    public InputAction visibilityToggleAction; // Input action for toggling visibility
    public GameObject[] eyeContactDeviceParts; // Array of parts to toggle visibility

    public CarControl2 carControl2; // Reference to CarControl2
    public CarControl3 carControl3; // Reference to CarControl3

    public GameObject detectionZone; // Reference to the DetectionZone GameObject

    private Quaternion initialCoverRotation; // To store the initial rotation of Unit_Cover
    private Quaternion initialSphereRotation; // To store the initial rotation of Unit_Sphere
    private bool devicesVisible = false; // Start with devices hidden by default

    void Start()
    {
        // Store the initial rotations
        initialCoverRotation = unitCover.localRotation;
        initialSphereRotation = unitSphere.localRotation;

        // Start with devices hidden
        SetDevicesVisibility(devicesVisible);

        // Enable the input action and register the callback
        visibilityToggleAction.Enable();
        visibilityToggleAction.performed += ToggleVisibility;
    }

    void Update()
    {
        // Enable or disable DetectionZone based on CarControl2 and device visibility
        if (carControl2.enabled && !carControl3.enabled && devicesVisible)
        {
            detectionZone.SetActive(true);

            // Calculate the distance to the player
            float distanceToPlayer = Vector3.Distance(transform.position, playerCamera.position);

            if (distanceToPlayer <= detectionRange)
            {
                // Calculate direction to the player
                Vector3 directionToPlayer = playerCamera.position - unitCover.position;

                // Horizontal rotation for Unit_Cover_Parent (only Y-axis)
                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                Vector3 eulerRotation = targetRotation.eulerAngles;
                unitCover.rotation = Quaternion.Slerp(unitCover.rotation, Quaternion.Euler(0, eulerRotation.y, 0), Time.deltaTime * 5f);

                // Vertical rotation for Unit_Sphere (up and down only)
                Vector3 localDirection = unitCover.InverseTransformPoint(playerCamera.position);
                float verticalAngle = -Mathf.Atan2(localDirection.y, localDirection.z) * Mathf.Rad2Deg; // Inverting the angle here

                // Apply rotation only on the X-axis for the Unit_Sphere
                Quaternion verticalRotation = Quaternion.Euler(verticalAngle, 0, 0);
                unitSphere.localRotation = Quaternion.Slerp(unitSphere.localRotation, verticalRotation, Time.deltaTime * 5f);
            }
            else
            {
                // Return to the initial rotations when out of range or during the animation
                unitCover.localRotation = Quaternion.Slerp(unitCover.localRotation, initialCoverRotation, Time.deltaTime * 5f);
                unitSphere.localRotation = Quaternion.Slerp(unitSphere.localRotation, initialSphereRotation, Time.deltaTime * 5f);
            }
        }
        else
        {
            // Disable DetectionZone and reset to initial rotations if CarControl2 is inactive, CarControl3 is active, or devices are hidden
            detectionZone.SetActive(false);
            unitCover.localRotation = Quaternion.Slerp(unitCover.localRotation, initialCoverRotation, Time.deltaTime * 5f);
            unitSphere.localRotation = Quaternion.Slerp(unitSphere.localRotation, initialSphereRotation, Time.deltaTime * 5f);
        }
    }

    private void ToggleVisibility(InputAction.CallbackContext context)
    {
        devicesVisible = !devicesVisible;
        SetDevicesVisibility(devicesVisible);
    }

    private void SetDevicesVisibility(bool visibility)
    {
        foreach (GameObject part in eyeContactDeviceParts)
        {
            part.SetActive(visibility);
        }

        // Toggle DetectionZone only when devices are visible and CarControl2 is active
        if (detectionZone != null)
        {
            detectionZone.SetActive(visibility && carControl2.enabled && !carControl3.enabled);
        }
    }

    private void OnDisable()
    {
        // Unregister the input action callback
        visibilityToggleAction.performed -= ToggleVisibility;
        visibilityToggleAction.Disable();
    }
}
