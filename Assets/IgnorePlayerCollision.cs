using UnityEngine;

public class IgnorePlayerCollision : MonoBehaviour
{
    public GameObject player; // Reference to the player GameObject

    private void Start()
    {
        // Get the colliders of the car and player
        Collider carCollider = GetComponent<Collider>();
        Collider playerCollider = player.GetComponent<Collider>();

        // Ensure both colliders exist
        if (carCollider != null && playerCollider != null)
        {
            // Ignore collisions between the car and player
            Physics.IgnoreCollision(carCollider, playerCollider);
        }
    }
}
