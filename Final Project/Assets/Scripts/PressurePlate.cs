using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [Header("Platform Settings")]
    public Transform platform;            // The object to move
    public Vector3 targetPosition;        // Desired position
    public float speed = 2f;              // Movement speed

    private Vector3 originalPosition;     // Original position of the platform
    private bool platformActivated = false; // Whether the platform is moving towards the target
    private bool returningToOriginal = false; // Whether the platform is moving back to the original position

    private void Start()
    {
        // Save the original position of the platform
        if (platform != null)
        {
            originalPosition = platform.position;
        }
    }

    private void Update()
    {
        if (platform != null)
        {
            // Move the platform towards the target position
            if (platformActivated)
            {
                platform.position = Vector3.MoveTowards(platform.position, targetPosition, speed * Time.deltaTime);

                // If the platform has reached the target position, deactivate movement
                if (Vector3.Distance(platform.position, targetPosition) < 0.01f)
                {
                    platformActivated = false;
                    Debug.Log("Platform reached target position.");
                }
            }

            // Move the platform back to the original position
            if (returningToOriginal)
            {
                platform.position = Vector3.MoveTowards(platform.position, originalPosition, speed * Time.deltaTime);

                // If the platform has returned to the original position, deactivate movement
                if (Vector3.Distance(platform.position, originalPosition) < 0.01f)
                {
                    returningToOriginal = false;
                    Debug.Log("Platform returned to original position.");
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Activate the platform movement towards the target position
        if (!platformActivated && !returningToOriginal)
        {
            platformActivated = true;
            Debug.Log($"Pressure plate activated by {other.gameObject.name}. Platform moving to target position: {targetPosition}");
        }
        else if (!returningToOriginal && !platformActivated)
        {
            // If the platform is at the target position, return to the original position
            returningToOriginal = true;
            Debug.Log($"Pressure plate activated again by {other.gameObject.name}. Platform returning to original position: {originalPosition}");
        }
    }
}
