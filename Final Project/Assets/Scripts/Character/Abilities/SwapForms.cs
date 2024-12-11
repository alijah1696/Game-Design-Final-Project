using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapForms : MonoBehaviour
{
    public GameObject[] forms; // Array to hold different forms
    private GameObject currentForm; // Reference to the current active form
    private Rigidbody2D currentRb; // Reference to the Rigidbody2D of the current form
    private Vector2 previousVelocity; // To store the previous velocity for acceleration calculation
    private Vector2 currentAcceleration; // To store the current acceleration

    // Start is called before the first frame update
    void Start()
    {
        if (forms.Length > 0)
        {
            currentForm = forms[0]; // Set the initial form
            currentRb = currentForm.GetComponent<Rigidbody2D>(); // Get the Rigidbody2D of the initial form
            ActivateForm(0); // Activate the initial form
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate acceleration
        currentAcceleration = (currentRb.velocity - previousVelocity) / Time.deltaTime;
        previousVelocity = currentRb.velocity;
    }

    // Helper method to get the current form
    public GameObject CurrentForm()
    {
        return (currentForm == null) ? forms[0] : currentForm;
    }

    // Method to swap forms (must be public so UI code can access)
    public void SwapForm(int formIndex)
    {
        if (formIndex < 0 || formIndex >= forms.Length) return;

        // Save the current position, velocity, and acceleration
        Vector3 currentPosition = currentForm.transform.position;
        Vector2 currentVelocity = currentRb.velocity;
        Vector2 currentAcceleration = (currentRb.velocity - previousVelocity) / Time.deltaTime;

        //Old variables
        MoveCharacter oldMv = currentForm.GetComponent<MoveCharacter>();

        // Deactivate all forms
        foreach (GameObject form in forms)
        {
            form.SetActive(false);
        }

        // Activate the selected form
        forms[formIndex].SetActive(true);
        currentForm = forms[formIndex];
        currentRb = currentForm.GetComponent<Rigidbody2D>();

        // Set the position, velocity, and acceleration of the new form
        currentForm.transform.position = currentPosition;
        currentRb.velocity = currentVelocity;
        // Apply the calculated acceleration (if needed, you might need to apply forces to simulate this)
        currentRb.AddForce(currentAcceleration * currentRb.mass, ForceMode2D.Force);

        //Transfer variables from old form
        MoveCharacter currentMv = currentForm.GetComponent<MoveCharacter>();
        currentMv.TransferVariablesFrom(oldMv);

        // Update previous velocity for the new form
        previousVelocity = currentRb.velocity;
    }

    // Method to activate a specific form
    private void ActivateForm(int formIndex)
    {
        if (formIndex < 0 || formIndex >= forms.Length) return;

        // Deactivate all forms
        foreach (GameObject form in forms)
        {
            form.SetActive(false);
        }

        // Activate the selected form
        forms[formIndex].SetActive(true);
        currentForm = forms[formIndex];
        currentRb = currentForm.GetComponent<Rigidbody2D>();

        // Initialize previous velocity for the new form
        previousVelocity = currentRb.velocity;
    }
}
