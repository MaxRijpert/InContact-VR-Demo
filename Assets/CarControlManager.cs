using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class CarControlManager : MonoBehaviour
{
    [SerializeField] private InputActionProperty startCarControl2Action;
    [SerializeField] private InputActionProperty startCarControl3Action;
    [SerializeField] private InputActionProperty resetPositionAction;

    [SerializeField] private CarControl2 carControl2;
    [SerializeField] private CarControl3 carControl3;
    [SerializeField] private Rigidbody carRigidbody; // Reference to the car's Rigidbody

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        startCarControl2Action.action.performed += _ => StartCarControl2();
        startCarControl3Action.action.performed += _ => StartCarControl3();
        resetPositionAction.action.performed += _ => ResetPosition();

        startCarControl2Action.action.Enable();
        startCarControl3Action.action.Enable();
        resetPositionAction.action.Enable();

        carControl2.enabled = false;
        carControl3.enabled = false;
    }

    void OnDestroy()
    {
        startCarControl2Action.action.performed -= _ => StartCarControl2();
        startCarControl3Action.action.performed -= _ => StartCarControl3();
        resetPositionAction.action.performed -= _ => ResetPosition();
    }

    void StartCarControl2()
    {
        Debug.Log("Starting Car Control 2 for " + gameObject.name);
        ResetCarControl();
        carControl2.ResetControl();
        carControl2.enabled = true;
    }

    void StartCarControl3()
    {
        Debug.Log("Starting Car Control 3 for " + gameObject.name);
        ResetCarControl();
        carControl3.ResetControl();
        carControl3.enabled = true;
    }

    void ResetPosition()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        // Immediately stop the car's movement
        if (carRigidbody != null)
        {
            carRigidbody.linearVelocity = Vector3.zero;
            carRigidbody.angularVelocity = Vector3.zero;
        }

        ResetCarControl();
    }

    private void ResetCarControl()
    {
        carControl2.enabled = false;
        carControl3.enabled = false;

        carControl2.ResetControl();
        carControl3.ResetControl();
    }
}
