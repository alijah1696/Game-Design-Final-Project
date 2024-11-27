using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [Header("Platform Settings")]          // Movement speed
    public string scene;
    private bool platformActivated = false; 
    private bool returningToOriginal = false;

    private void Start()
    {
    
    }

    private void Update()
    {
        // Move the platform towards the target position
        if (platformActivated)
        {
            SceneManager.LoadScene(scene);
            Time.timeScale = 1.0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!platformActivated && !returningToOriginal)
        {
            platformActivated = true;
            Debug.Log($"Pressure plate activated by {other.gameObject.name}.");
        }
    }
}
