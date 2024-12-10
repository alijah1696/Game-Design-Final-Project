using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFrameRateChanger : MonoBehaviour
{
    public Camera targetCamera; // The camera to control
    public int targetFrameRate = 10; // Desired frame rate for the camera

    private float interval; // Time interval between frames
    private float nextFrameTime; // Time for the next frame update

    void Start()
    {
        if (targetCamera != null)
        {
            // Calculate the interval based on the target frame rate
            interval = 1.0f / targetFrameRate;
        }
        else
        {
            Debug.LogError("Target camera is not assigned.");
        }
    }

    void Update()
    {
        if (Time.time >= nextFrameTime)
        {
            // Render the camera
            targetCamera.enabled = true;
            targetCamera.Render();
            targetCamera.enabled = false;

            // Set the time for the next frame update
            nextFrameTime = Time.time + interval;
        }
    }
}
