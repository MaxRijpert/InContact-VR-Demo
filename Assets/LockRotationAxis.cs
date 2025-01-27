using UnityEngine;

public class LockTransform : MonoBehaviour
{
    // Rotation locking options
    public bool lockRotationX = true;
    public bool lockRotationY = false;
    public bool lockRotationZ = false;

    // Position locking options
    public bool lockPositionX = false;
    public bool lockPositionY = true;  // Set to true to keep the shadow at the same height
    public bool lockPositionZ = false;

    private Vector3 initialRotation;
    private Vector3 initialPosition;

    void Start()
    {
        // Store the initial rotation and position
        initialRotation = transform.rotation.eulerAngles;
        initialPosition = transform.position;
    }

    void Update()
    {
        // Handle rotation locking
        Vector3 currentRotation = transform.rotation.eulerAngles;
        if (lockRotationX) currentRotation.x = initialRotation.x;
        if (lockRotationY) currentRotation.y = initialRotation.y;
        if (lockRotationZ) currentRotation.z = initialRotation.z;
        transform.rotation = Quaternion.Euler(currentRotation);

        // Handle position locking
        Vector3 currentPosition = transform.position;
        if (lockPositionX) currentPosition.x = initialPosition.x;
        if (lockPositionY) currentPosition.y = initialPosition.y;
        if (lockPositionZ) currentPosition.z = initialPosition.z;
        transform.position = currentPosition;
    }
}
