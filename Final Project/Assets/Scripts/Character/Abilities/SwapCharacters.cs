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
        // Start with the plant as the active character by default
        activeCharacter = plant;
        SetActiveCharacter(plant);

        // Ensure the other character is inactive
        robot.SetActive(false);
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
        // Transfer position
        characterToActivate.transform.position = activeCharacter.transform.position;

        // Transfer variables
        Rigidbody2D currentRb = activeCharacter.GetComponent<Rigidbody2D>();
        Rigidbody2D newRb = characterToActivate.GetComponent<Rigidbody2D>();
        
        MoveCharacter currentMoveVars = activeCharacter.GetComponent<MoveCharacter>();
        MoveCharacter newMoveVars = characterToActivate.GetComponent<MoveCharacter>();
        
        newMoveVars.transferVariablesFrom(currentMoveVars);

        newRb.velocity = currentRb.velocity;

        // Deactivate the previous character and activate the new one
        activeCharacter.SetActive(false);
        characterToActivate.SetActive(true);

        // Update the active character reference
        activeCharacter = characterToActivate;
    }

    public GameObject getActiveCharacter(){
        return activeCharacter;
    }
}
