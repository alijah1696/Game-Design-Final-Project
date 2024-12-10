using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLogic : MonoBehaviour
{
    private InteractableProxy proxy; // Handles interaction with various objects (e.g., pressure plates)
    private Collider2D c2d;
    private SpriteRenderer sr;

    public bool isOpen;

    [SerializeField]
    private Sprite openedSprite; // Sprite for the open state
    private Sprite closedSprite; // Sprite for the closed state

    private AudioManager audioManager; // Reference to AudioManager

    void Start()
    {
        proxy = GetComponent<InteractableProxy>();
        sr = GetComponent<SpriteRenderer>();
        c2d = GetComponent<Collider2D>();

        closedSprite = sr.sprite;

        // Find the AudioManager
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found! Make sure it exists in the scene.");
        }
    }

    void Update()
    {
        // Get the progress from the proxy (e.g., pressure plate, button)
        float progress = proxy.getProgress();

        // Open or close the door based on progress
        if (progress == 1 && !isOpen)
        {
            Open();
        }
        else if (progress == 0 && isOpen)
        {
            Close();
        }
    }

    public void Open()
    {
        isOpen = true;
        c2d.isTrigger = true; // Make the collider a trigger to allow passage
        sr.sprite = openedSprite; // Change the sprite to indicate the open state

        // Play the door opening sound
        if (audioManager != null && audioManager.pressurePlateSound != null)
        {
            audioManager.PlaySFX(audioManager.pressurePlateSound);
            Debug.Log("Door opened sound played.");
        }

        Debug.Log("Door opened.");
    }

    public void Close()
    {
        isOpen = false;
        c2d.isTrigger = false; // Make the collider solid again
        sr.sprite = closedSprite; // Change the sprite to indicate the closed state

        // Play the door closing sound
        if (audioManager != null && audioManager.pressurePlateSound != null)
        {
            audioManager.PlaySFX(audioManager.pressurePlateSound);
            Debug.Log("Door closed sound played.");
        }

        Debug.Log("Door closed.");
    }
}
