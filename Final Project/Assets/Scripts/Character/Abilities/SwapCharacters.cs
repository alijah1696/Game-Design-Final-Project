using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SwapCharacters : MonoBehaviour
{
    public GameObject plant;      // The first character
    public GameObject robot;      // The second character
    private GameObject activeCharacter; // Reference to the currently active character
    private AudioManager audioManager;

    private int numLives;
    [SerializeField] private int Lives;

    private int formIndex;

    private void Awake()
    {
        // Find the AudioManager in the scene and get its component
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("SwapCharacters: AudioManager not found! Make sure an AudioManager exists in the scene.");
        }
    }

    private void Start()
    {
        // Set the initial active character to the plant and disable the robot
        activeCharacter = plant;
        plant.SetActive(true);
        robot.SetActive(false);

        numLives = Lives;

        LoseScreen.previousScene = SceneManager.GetActiveScene().name;
    }

    public bool IsPlantActive()
    {
        return (activeCharacter == plant);
    }

    public bool IsFacingRight()
    {
        return GetCurrentForm().GetComponent<MoveCharacter>().GetDirection();
    }

    private void Update(){

    }

    private void PlaySwitchSound()
    {
        if (audioManager != null && audioManager.switchingCharacterSound != null)
        {
            Debug.Log("SwapCharacters: Playing switching character sound.");
            audioManager.PlaySFX(audioManager.switchingCharacterSound);
        }
        else
        {
            Debug.LogWarning("SwapCharacters: Switching character sound or AudioManager is null.");
        }
    }

    public GameObject GetCurrentForm()
    {
        return activeCharacter.GetComponent<SwapForms>().CurrentForm();
    }

    public void SwapCharacter()
    {
        GameObject oldCharacter = activeCharacter;
        activeCharacter.SetActive(false);
        activeCharacter = (activeCharacter == plant) ? robot : plant;
        activeCharacter.SetActive(true);
        
        TransferVariables(oldCharacter);
        PlaySwitchSound();
    }

    public void TransferVariables(GameObject old)
    {
        SwapForms oldSf = old.GetComponent<SwapForms>();
        SwapForms activeSf = activeCharacter.GetComponent<SwapForms>();

        MoveCharacter oldMv = oldSf.CurrentForm().GetComponent<MoveCharacter>();
        MoveCharacter activeMv = activeSf.CurrentForm().GetComponent<MoveCharacter>();

        activeMv.TransferVariablesFrom(oldMv);
        activeSf.CurrentForm().transform.position = oldSf.CurrentForm().transform.position;

        Debug.Log("SwapCharacters: Transferred variables between characters.");
    }

    public void LooseScreen()
    {
        Debug.Log("SwapCharacters: Loading lose screen.");
        SceneManager.LoadScene("LoseMenu");
    }

    public void Kill(GameObject respawnPoint)
    {
        if (numLives > 1)
        {
            Debug.Log("SwapCharacters: Respawning character...");
            GameObject character = GetCurrentForm();
            MoveCharacter mv = character.GetComponent<MoveCharacter>();
            if (activeCharacter == plant)
            {
                PlantVineMovement vm = character.GetComponent<PlantVineMovement>();
                vm.EndSwing();
            }
            else if (activeCharacter == robot)
            {
                MagneticAbilities ma = character.GetComponent<MagneticAbilities>();
                ma.StopControl();
            }
            character.transform.position = respawnPoint.transform.position;
            character.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        else
        {
            Debug.Log("SwapCharacters: No lives left, loading lose screen.");
            LooseScreen();
        }
        numLives--;
    }

    public float CurrentLives()
    {
        return numLives;
    }

    public bool isCurrentlyBusy()
    {
        return GetCurrentForm().GetComponent<MoveCharacter>().isBusy;
    }

    public float GetCurrentIndex()
    {
        return formIndex;
    }

    public void SetPlantForm1() { plant.GetComponent<SwapForms>().SwapForm(0); formIndex = 1; }
    public void SetPlantForm2() { plant.GetComponent<SwapForms>().SwapForm(1); formIndex = 0; }
    public void SetPlantForm3() { plant.GetComponent<SwapForms>().SwapForm(2); formIndex = 2; }

    public void SetRobotForm1() { robot.GetComponent<SwapForms>().SwapForm(0); formIndex = 4; }
    public void SetRobotForm2() { robot.GetComponent<SwapForms>().SwapForm(1); formIndex = 5; }
    public void SetRobotForm3() { robot.GetComponent<SwapForms>().SwapForm(2); formIndex = 3; }
}
