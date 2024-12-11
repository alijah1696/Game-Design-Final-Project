using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapCharacters : MonoBehaviour
{
    public GameObject plant;      // The first character
    public GameObject robot;      // The second character
    private GameObject activeCharacter; // Reference to the currently active character
    private AudioManager audioManager;

    private int formIndex;

    private void Awake()
    {
        // Find the AudioManager in the scene and get its component
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
    }

    private void Start()
    {
        // Set the initial active character to the plant and disable the robot
        activeCharacter = plant;
        plant.SetActive(true);
        robot.SetActive(false);
    }

    public bool IsPlantActive(){
        return (activeCharacter == plant);
    }

    public bool IsFacingRight(){
        return GetCurrentForm().GetComponent<MoveCharacter>().GetDirection();
    }

    private void Update()
    {
        // Check if the character is on the ground and if the Tab key is pressed to swap characters
        // bool isOnGround = GetCurrentForm().GetComponent<MoveCharacter>().IsOnGround();
        // if (Input.GetKeyDown(KeyCode.Tab) && isOnGround && false)
        // {
        //     SwapCharacter();

        //     // Play character switch sound, if available
        //     if (audioManager != null && audioManager.switchingCharacterSound != null)
        //     {
        //         audioManager.PlaySFX(audioManager.switchingCharacterSound);
        //     }
        // }
    }

    // Public method to get the currently active character for other scripts (e.g., CameraFollow)
    public GameObject GetCurrentForm()
    {
        return activeCharacter.GetComponent<SwapForms>().CurrentForm();
    }

    //needs to be public so UI buttons can access:
    public void SwapCharacter()
    {
        // Toggle between plant and robot as the active character
        GameObject oldCharacter = activeCharacter;
        activeCharacter.SetActive(false);
        activeCharacter = (activeCharacter == plant) ? robot : plant;
        activeCharacter.SetActive(true);

        TransferVariables(oldCharacter);
    }

    // Transfer variables from the old character to the new one
    public void TransferVariables(GameObject old)
    {   
        SwapForms oldSf = old.GetComponent<SwapForms>();
        SwapForms activeSf = activeCharacter.GetComponent<SwapForms>();

        MoveCharacter oldMv = oldSf.CurrentForm().GetComponent<MoveCharacter>();
        MoveCharacter activeMv = activeSf.CurrentForm().GetComponent<MoveCharacter>();

        // Transfer movement variables and position from old to new character
        activeMv.TransferVariablesFrom(oldMv);
        activeSf.CurrentForm().transform.position = oldSf.CurrentForm().transform.position;
        
    }

    public bool isCurrentlyBusy(){
        return GetCurrentForm().GetComponent<MoveCharacter>().isBusy;
    }

    public float GetCurrentIndex(){
        return formIndex;
    }

    //these functions allow the UI to access forms swaps:
    public void SetPlantForm1(){plant.GetComponent<SwapForms>().SwapForm(0); formIndex = 1;}
    public void SetPlantForm2(){plant.GetComponent<SwapForms>().SwapForm(1); formIndex = 0;}
    public void SetPlantForm3(){plant.GetComponent<SwapForms>().SwapForm(2); formIndex = 2;}

    public void SetRobotForm1(){robot.GetComponent<SwapForms>().SwapForm(0); formIndex = 4;}
    public void SetRobotForm2(){robot.GetComponent<SwapForms>().SwapForm(1); formIndex = 5;}
    public void SetRobotForm3(){robot.GetComponent<SwapForms>().SwapForm(2); formIndex = 3;}

}
