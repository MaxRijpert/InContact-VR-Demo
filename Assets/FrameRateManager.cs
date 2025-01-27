using UnityEngine;

public class FrameRateManager : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = 72; // Locks the frame rate to 72 FPS


        if (OVRManager.isHmdPresent)
        {
            OVRManager.fixedFoveatedRenderingLevel = OVRManager.FixedFoveatedRenderingLevel.Medium; // Maximum foveation
        }
    }
}
