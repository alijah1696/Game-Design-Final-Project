using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target; // The GameObject that holds the SwapCharacters script
    private GameObject activeCharacter; // Reference to the active character
    public float followSpeed = 0.1f; // Speed at which the camera follows the character
    public Vector3 offset = new Vector3(0, 2, -200); // Offset from the active character's position
    private Vector3 velocity = Vector3.zero; // Used for SmoothDamp

    bool followTempTarget = false;

    void Update() // Use LateUpdate for smoother following
    {
        // Retrieve the SwapCharacters script from the target GameObject
        
        SwapCharacters swapScript = target.GetComponent<SwapCharacters>();
        if (swapScript != null && !followTempTarget)
        {
            // Get the currently active character from SwapCharacters
            activeCharacter = swapScript.GetCurrentForm();
        }   

        // Check if activeCharacter is not null before following
        if (activeCharacter != null)
        {
            // Calculate the target position for the camera
            Vector3 targetPosition = activeCharacter.transform.position + offset;


            // Smoothly move the camera towards the target position
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, followSpeed);
        }
        else
        {
            Debug.LogWarning("Active character is null. Camera cannot follow.");
        }
    }

    public void FollowTemporaryTarget(GameObject temptarget){
        followTempTarget = true;
        activeCharacter = temptarget;
    }

    public void FollowPlayer(){
        followTempTarget = false;
    }
}
