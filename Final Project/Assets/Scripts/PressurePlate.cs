using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private AudioManager audioManager;    // Reference to the AudioManager
    public DoorLogic connectedDoor;       // Reference to the connected door logic (set in the Inspector)

    private bool isActivated = false;     // Tracks if the pressure plate is currently activated

    // Public getter for activation status
    public bool IsActivated => isActivated;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found! Make sure it exists in the scene.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActivated)
        {
            isActivated = true;

            // Play the pressure plate sound
            if (audioManager != null && audioManager.pressurePlateSound != null)
            {
                audioManager.PlaySFX(audioManager.pressurePlateSound);
                Debug.Log($"Pressure plate activated by {other.gameObject.name}. Sound played.");
            }

            // Notify the connected door to open
            if (connectedDoor != null)
            {
                connectedDoor.Open();
                Debug.Log("Connected door opened by pressure plate.");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (isActivated)
        {
            isActivated = false;

            // Optionally play sound when deactivating the pressure plate
            if (audioManager != null && audioManager.pressurePlateSound != null)
            {
                audioManager.PlaySFX(audioManager.pressurePlateSound);
                Debug.Log($"Pressure plate deactivated by {other.gameObject.name}. Sound played.");
            }

            // Optionally notify the connected door to close
            // Uncomment the following lines if you want the door to close when the plate is deactivated
            /*
            if (connectedDoor != null)
            {
                connectedDoor.Close();
                Debug.Log("Connected door closed by pressure plate.");
            }
            */
        }
    }
}
