using UnityEngine;

public class DeviceSoundController : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayBlinkSound()
    {
        if (!audioSource.isPlaying) // To prevent overlapping sounds
        {
            audioSource.Play();
        }
    }

    public void StopBlinkSound()
    {
        audioSource.Stop();
    }
}
