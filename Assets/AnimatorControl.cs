using UnityEngine;

public class AnimatorControl : MonoBehaviour
{
    public Animator[] animatorsToControl;   // Assign animators in Inspector
    public GameObject detectionZone;        // Assign the DetectionZone GameObject in Inspector

    private CarControl3 carControl3Script;
    private CarControl2 carControl2Script;

    void Start()
    {
        // Get references to the CarControl scripts
        carControl2Script = FindObjectOfType<CarControl2>();
        carControl3Script = FindObjectOfType<CarControl3>();

        // Ensure that both car control scripts are found
        if (carControl2Script == null || carControl3Script == null)
        {
            Debug.LogError("CarControl scripts not found in the scene.");
            return;
        }

        // Set initial state based on which script is active
        UpdateAnimatorAndZoneState();
    }

    void Update()
    {
        // Continuously check the active script and update animators and DetectionZone state
        UpdateAnimatorAndZoneState();
    }

    private void UpdateAnimatorAndZoneState()
    {
        if (carControl3Script != null && carControl3Script.isActiveAndEnabled)
        {
            // CarControl3 is active: deactivate DetectionZone and reset animator parameters
            SetDetectionZoneActive(false);
            ResetInDetectionZoneParameters();
        }
        else if (carControl2Script != null && carControl2Script.isActiveAndEnabled)
        {
            // CarControl2 is active: activate DetectionZone
            SetDetectionZoneActive(true);
        }
    }

    // Activate or deactivate the DetectionZone GameObject
    private void SetDetectionZoneActive(bool isActive)
    {
        if (detectionZone != null)
        {
            detectionZone.SetActive(isActive);
        }
    }

    // Set the "InDetectionZone" parameter to false for all animators
    private void ResetInDetectionZoneParameters()
    {
        foreach (var animator in animatorsToControl)
        {
            if (animator != null)
            {
                animator.SetBool("InDetectionZone", false);
            }
        }
    }
}
