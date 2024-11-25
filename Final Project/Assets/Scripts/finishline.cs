using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    [Header("Next Level Settings")]
    public string nextSceneName; // Name of the next scene
    public bool loadNextSceneByIndex = false; // Whether to load the next scene by build index

    private SwapCharacters swapCharacters; // Reference to the SwapCharacters script

    private void Start()
    {
        // Find the SwapCharacters script in the scene
        swapCharacters = FindObjectOfType<SwapCharacters>();

        if (swapCharacters == null)
        {
            Debug.LogError("SwapCharacters script not found in the scene. Ensure it's attached to the GameObject managing character swapping.");
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Check if the object colliding is the currently active character
        if (swapCharacters != null && other.gameObject == swapCharacters.GetCurrentForm())
        {
            Debug.Log($"FinishLine triggered by: {other.gameObject.name}");

            // Load the next scene based on settings
            if (loadNextSceneByIndex)
            {
                int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
                if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
                {
                    Debug.Log($"Loading scene by index: {nextSceneIndex}");
                    SceneManager.LoadScene(nextSceneIndex);
                }
                else
                {
                    Debug.LogError("No next scene in Build Settings. Check your scene order.");
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(nextSceneName))
                {
                    Debug.Log($"Loading scene by name: {nextSceneName}");
                    SceneManager.LoadScene(nextSceneName);
                }
                else
                {
                    Debug.LogError("Next scene name is not set. Please specify it in the Inspector.");
                }
            }
        }
        else
        {
            Debug.Log($"FinishLine ignored interaction with {other.gameObject.name}. Ensure it is the active character.");
        }
    }
}
