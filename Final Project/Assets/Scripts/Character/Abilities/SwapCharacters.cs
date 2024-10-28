using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapCharacters : MonoBehaviour
{
    public GameObject plant;      // The first character (mutated plant)
    public GameObject robot;      // The second character (robot)

    private GameObject activeCharacter; // Currently active character

    void Start()
    {
        // Ensure the other character is inactive
        robot.SetActive(false);

        // Start with the plant as the active character by default
        activeCharacter = plant;
    }

    void Update()
    {
        // Swap characters when Tab is pressed
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwapCharacter();
        }
    }

    void SwapCharacter()
    {
        // Toggle the active character
        if (activeCharacter == plant)
        {
            SetActiveCharacter(robot);
        }
        else
        {
            SetActiveCharacter(plant);
        }
    }

    void SetActiveCharacter(GameObject characterToActivate)
    {

        // Stops method if any character is null
        if(activeCharacter == null || characterToActivate == null) return;

        // Transfer variables

        SwapForms currentForm = activeCharacter.GetComponent<SwapForms>();
        SwapForms newForm = characterToActivate.GetComponent<SwapForms>();

        Rigidbody2D currentRb = currentForm.CurrentForm().GetComponent<Rigidbody2D>();
        Rigidbody2D newRb = newForm.CurrentForm().GetComponent<Rigidbody2D>();

        MoveCharacter currentMoveVars = currentForm.CurrentForm().GetComponent<MoveCharacter>();
        MoveCharacter newMoveVars = newForm.CurrentForm().GetComponent<MoveCharacter>();

        newMoveVars.transferVariablesFrom(currentMoveVars);

        // Deactivate the previous character and activate the new one
        activeCharacter.SetActive(false);
        characterToActivate.SetActive(true);

        // Transfer position and velocity
        newForm.CurrentForm().transform.position = currentForm.CurrentForm().transform.position;
        characterToActivate.transform.position = activeCharacter.transform.position;
        newRb.velocity = currentRb.velocity;

        // Update the active character reference
        activeCharacter = characterToActivate;
    }

    public GameObject getActiveCharacter(){
        return activeCharacter;
    }

    public GameObject getCurrentForm(){
        return activeCharacter.GetComponent<SwapForms>().CurrentForm();
    }
}
