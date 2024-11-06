using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapCharacters : MonoBehaviour
{
    public GameObject plant;      // The first character
    public GameObject robot;      // The second character
    private GameObject activeCharacter; // Reference to the currently active character
    private AudioManager audioManager;

    private void Awake()
    {
        // Find the AudioManager in the scene and get its component
        audioManager = null;
    }

    private void Start()
    {
        // Set the initial active character to the plant and disable the robot
        activeCharacter = plant;
        plant.SetActive(true);
        robot.SetActive(false);
    }

    private void Update()
    {
        // Check for Tab key press to swap characters

        
        bool isOnGround = GetCurrentForm().GetComponent<MoveCharacter>().IsOnGround();
        if (Input.GetKeyDown(KeyCode.Tab) && isOnGround)
        {
            SwapCharacter();
            // Play character switch sound, if available
            if (audioManager != null && audioManager.switchingCharacterSound != null)
            {
                audioManager.PlaySFX(audioManager.switchingCharacterSound);
            }
        }
    }


    // Public method to get the currently active character for other scripts (e.g., CameraFollow)
    public GameObject GetCurrentForm()
    {
        return activeCharacter.GetComponent<SwapForms>().CurrentForm();
    }

    private void SwapCharacter()
    {
        // Toggle between plant and robot as the active character
        
        GameObject OldCharacter= activeCharacter;
        activeCharacter.SetActive(false);
        activeCharacter = (activeCharacter == plant) ? robot : plant;
        activeCharacter.SetActive(true);

        TransferVarables(OldCharacter);

    }

    public void TransferVarables(GameObject old){

        SwapForms oldSf = old.GetComponent<SwapForms>();
        SwapForms activeSf = activeCharacter.GetComponent<SwapForms>();

        MoveCharacter oldMv = oldSf.CurrentForm().GetComponent<MoveCharacter>();
        MoveCharacter activeMv = activeSf.CurrentForm().GetComponent<MoveCharacter>();

        activeMv.TransferVariablesFrom(oldMv);
        activeSf.CurrentForm().transform.position = oldSf.CurrentForm().transform.position;
    }
}
