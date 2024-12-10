using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLogic : MonoBehaviour
{
    private Collider2D c2d;
    private SpriteRenderer sr;

    public bool isOpen;

    [SerializeField]
    private Sprite openedSprite; // Sprite for the open state
    private Sprite closedSprite; // Sprite for the closed state

    private AudioManager audioManager; // Reference to AudioManager

    public PressurePlate connectedPressurePlate; // Reference to the connected pressure plate
    public InteractableProxy interactableProxy; // Reference to InteractableProxy (for key or other interactables)
    public bool keyUsed = false; // Tracks if the key has been used to open the door

    private bool pressurePlateTriggered = false; // Tracks if the pressure plate has been activated

    void Start()
    {
        Debug.Log("DoorLogic: Start method called.");
        sr = GetComponent<SpriteRenderer>();
        c2d = GetComponent<Collider2D>();

        closedSprite = sr.sprite;

        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found! Make sure it exists in the scene.");
        }
    }

    void Update()
    {
        Debug.Log("DoorLogic: Update method called.");

        // Check if the door should open via pressure plate or interactable
        if ((connectedPressurePlate != null && connectedPressurePlate.IsActivated && !pressurePlateTriggered) ||
            (interactableProxy != null && interactableProxy.getProgress() == 1) ||
            keyUsed)
        {
            if (!isOpen)
            {
                Debug.Log("DoorLogic: Conditions met to open the door.");
                Open();

                // If the pressure plate activated the door, mark it as triggered
                if (connectedPressurePlate != null && connectedPressurePlate.IsActivated)
                {
                    pressurePlateTriggered = true;
                }
            }
        }
    }

    public void UseKey()
    {
        Debug.Log("DoorLogic: UseKey called.");
        keyUsed = true;
        Open();
    }

    public void Open()
    {
        if (!isOpen)
        {
            Debug.Log("DoorLogic: Opening the door.");
            isOpen = true;
            c2d.isTrigger = true;
            sr.sprite = openedSprite;

            if (audioManager != null && audioManager.pressurePlateSound != null)
            {
                audioManager.PlaySFX(audioManager.pressurePlateSound);
                Debug.Log("DoorLogic: Door opened sound played.");
            }
        }
        else
        {
            Debug.Log("DoorLogic: Door is already open.");
        }
    }

    public void Close()
    {
        // The door will no longer close automatically
        Debug.Log("DoorLogic: Close method called, but door will remain open.");
    }
}
