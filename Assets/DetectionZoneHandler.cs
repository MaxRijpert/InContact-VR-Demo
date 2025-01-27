using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    // List of animators to control manually
    public List<Animator> animatorsToControl;

    // The specific tag to filter colliders
    public string targetTag = "Unit_Sphere"; // Ensure this matches the tag assigned to your Unit_Sphere objects

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            Debug.Log($"Trigger entered by: {other.name}");
            foreach (Animator animator in animatorsToControl)
            {
                if (animator != null)
                {
                    animator.SetBool("InDetectionZone", true);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            Debug.Log($"Trigger exited by: {other.name}");
            foreach (Animator animator in animatorsToControl)
            {
                if (animator != null)
                {
                    animator.SetBool("InDetectionZone", false);
                }
            }
        }
    }
}

