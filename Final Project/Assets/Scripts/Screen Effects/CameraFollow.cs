using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target; // The target GameObject to find SwapCharacters on
    private GameObject activeCharacter; // Reference to the active character
    public float followSpeed = 0.25f; // Speed at which the camera follows the character
    public Vector3 offset; // Offset from the active character's position
    private Vector3 velocity = Vector3.zero; // Used for SmoothDamp

    void Start()
    {
        offset = new Vector3(0, -1, -10); // Set a reasonable offset
    }
    
    void Update() // Use LateUpdate for smoother following
    {
        //Update the active character each frame
        activeCharacter = target.GetComponent<SwapCharacters>().getCurrentForm();  
        // Check if activeCharacter is not null before following
        if (activeCharacter != null)
        {
            // Set the target position
            Vector3 targetPosition = activeCharacter.transform.position + offset;

            // Smoothly move the camera towards the target position
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, followSpeed);
        }
    }
}
