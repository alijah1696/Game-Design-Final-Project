using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicGas : MonoBehaviour
{
    public GameObject respawnPoint;  // The respawn point if the player dies
    private SwapCharacters sc;       // Reference to the character swapping script
    private AudioManager audioManager;  // Reference to AudioManager for playing sounds
    private bool isPlantInGas = false;  // To track whether the plant is still in the gas
    private bool isRobotInGas = false;  // To track whether the robot is still in the gas

    void Start()
    {
        sc = FindObjectOfType<SwapCharacters>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Plant") || other.CompareTag("Robot"))
        {
            StartGasEffects();
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Plant"))
        {
            isPlantInGas = true;  // Confirm the plant is still in the gas
        }
        else if (other.CompareTag("Robot"))
        {
            isRobotInGas = true;  // Confirm the robot is still in the gas
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Plant"))
        {
            StopGasEffects();
            isPlantInGas = false;
        }
        else if (other.CompareTag("Robot"))
        {
            StopGasEffects();
            isRobotInGas = false;
        }
    }

    void Update()
    {
        // Check to ensure that the gas sound continues if the characters are stationary
        if ((isPlantInGas || isRobotInGas) && !audioManager.IsPlayingToxicGasSound())
        {
            StartGasEffects();
        }

        // Reset flags unless OnTriggerStay2D sets them again
        isPlantInGas = false;
        isRobotInGas = false;
    }

    private void StartGasEffects()
    {
        if (!audioManager.IsPlayingToxicGasSound())
        {
            audioManager.PlayToxicGasSound();
        }
    }

    private void StopGasEffects()
    {
        audioManager.StopToxicGasSound();
    }
}
