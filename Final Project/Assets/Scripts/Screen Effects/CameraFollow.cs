using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target; // The GameObject that holds the SwapCharacters script
    private GameObject activeCharacter; // Reference to the active character
    public float followSpeed = 0.25f; // Speed at which the camera follows the character
    public Vector3 offset = new Vector3(0, -1, -10); // Offset from the active character's position
    private Vector3 velocity = Vector3.zero; // Used for SmoothDamp

    void LateUpdate() // Use LateUpdate for smoother following
    {
        // Retrieve the SwapCharacters script from the target GameObject
        SwapCharacters swapScript = target.GetComponent<SwapCharacters>();
        if (swapScript != null)
        {
            // Get the currently active character from SwapCharacters
            activeCharacter = swapScript.getCurrentForm();
        }

        // Check if activeCharacter is not null before following
        if (activeCharacter != null)
        {
            // Calculate the target position for the camera
            Vector3 targetPosition = activeCharacter.transform.position + offset;

            // Log the target position to the console for debugging
            Debug.Log("Camera Target Position: " + targetPosition);

            // Smoothly move the camera towards the target position
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, followSpeed);
        }
        else
        {
            Debug.LogWarning("Active character is null. Camera cannot follow.");
        }
    }
}
