using UnityEngine;

public class PlayMultipleAnimations : MonoBehaviour
{
    private Animation anim;

    void Start()
    {
        anim = GetComponent<Animation>();

        foreach (AnimationState state in anim)
        {
            anim.Play(state.name);
        }
    }
}
