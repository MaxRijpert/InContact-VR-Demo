using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class CarAnimationControl : MonoBehaviour
{
    private Animator animator;

    // InputActionProperty fields for each animation action
    [SerializeField] private InputActionProperty playAnimation1Action;
    [SerializeField] private InputActionProperty resetAnimation1Action;
    [SerializeField] private InputActionProperty playAnimation2Action; // For CarDrivingAnimation3

    void Start()
    {
        animator = GetComponent<Animator>();

        // Register input actions for the first animation
        playAnimation1Action.action.performed += _ => PlayAnimation("CarDrivingAnimation4");
        resetAnimation1Action.action.performed += _ => ResetAnimation("CarDrivingAnimation4");

        // Register input action for the second animation
        playAnimation2Action.action.performed += _ => PlayAnimation("CarDrivingAnimation3");

        // Enable the actions
        playAnimation1Action.action.Enable();
        resetAnimation1Action.action.Enable();
        playAnimation2Action.action.Enable();
    }

    void OnDestroy()
    {
        // Unregister input actions to prevent memory leaks
        playAnimation1Action.action.performed -= _ => PlayAnimation("CarDrivingAnimation4");
        resetAnimation1Action.action.performed -= _ => ResetAnimation("CarDrivingAnimation4");
        playAnimation2Action.action.performed -= _ => PlayAnimation("CarDrivingAnimation3");
    }

    void PlayAnimation(string animationName)
    {
        animator.SetBool("PlayAnimation", true);
        animator.SetBool("StopAnimation", false);
        animator.Play(animationName);
    }

    void ResetAnimation(string animationName)
    {
        animator.SetBool("StopAnimation", true);
        animator.SetBool("PlayAnimation", false);
        animator.Play(animationName, 0, 0f);
    }
}
