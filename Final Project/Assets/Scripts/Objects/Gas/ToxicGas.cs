using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicGas : MonoBehaviour
{
    public GameObject respawnPoint;
    private SwapCharacters sc;
    private AudioManager audioManager; // Reference to AudioManager

    private bool isRobotInGas = false; // Flag to track if the robot is in gas

    void Start()
    {
        // Find references for SwapCharacters and AudioManager
        sc = FindObjectOfType<SwapCharacters>();
        audioManager = FindObjectOfType<AudioManager>();

        if (audioManager == null)
        {
            Debug.LogWarning("ToxicGas: AudioManager not found in the scene.");
        }
    }

    void Update()
    {
        // If the robot is in gas, ensure the sound plays continuously
        if (isRobotInGas && audioManager != null && !audioManager.IsGasSoundPlaying())
        {
            audioManager.PlayGasSound();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Plant"))
        {
            sc.Kill(respawnPoint); // Kill logic for Plant
        }
        else if (other.CompareTag("Robot"))
        {
            MoveCharacter mv = sc.GetCurrentForm().GetComponent<MoveCharacter>();
            mv.InDanger();

            // Start gas sound and set flag
            isRobotInGas = true;
            if (audioManager != null && !audioManager.IsGasSoundPlaying())
            {
                audioManager.PlayGasSound();
                Debug.Log("ToxicGas: Gas sound started.");
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Robot"))
        {
            MoveCharacter mv = sc.GetCurrentForm().GetComponent<MoveCharacter>();
            mv.Safe();

            // Stop gas sound and reset flag
            isRobotInGas = false;
            if (audioManager != null)
            {
                audioManager.StopGasSound();
                Debug.Log("ToxicGas: Gas sound stopped.");
            }
        }
    }
}
