using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticAbilities : MonoBehaviour
{
    [Header("Ability Settings")]
    [Tooltip("Set the duration (in seconds) for how long the magnetic ability lasts.")]
    [Min(0.1f)] // Ensures duration is always positive
    public float abilityDuration = 5f; // Duration of the ability (editable in the Inspector)
    private float abilityTimer = 0f;   // Tracks remaining time for the ability

    private bool canControl;
    private bool isControlling;
    private bool shouldControl;

    private float verticalInput;
    private float horizontalInput;

    private CameraFollow camera;
    private MoveCharacter mv;
    private Rigidbody2D rb;

    private AudioManager audioManager; // Reference to AudioManager

    public float moveSpeed = 1f;
    public float moveTime = 0.1f;
    private Vector2 moveVelocity = Vector2.zero;

    public Color highlightColor;
    private Color defaultColor;

    private GameObject controlled;
    private bool isMagneticSoundPlaying = false; // Tracks if the magnetic sound is playing

    void Start()
    {
        mv = GetComponent<MoveCharacter>();
        rb = GetComponent<Rigidbody2D>();
        camera = FindObjectOfType<CameraFollow>();
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
    }

    void Update()
    {
        // Activate the magnetic ability
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (canControl && !isControlling)
            {
                StartControl();
            }
            else if (isControlling)
            {
                shouldControl = false;
            }
        }

        verticalInput = Input.GetAxisRaw("Vertical");
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Handle the ability timer
        if (isControlling)
        {
            abilityTimer -= Time.deltaTime;

            if (abilityTimer <= 0f)
            {
                StopControl(); // Automatically stop control when the timer expires
                Debug.Log("Magnetic ability timed out.");
            }
        }
    }

    void FixedUpdate()
    {
        if (isControlling && shouldControl)
        {
            Control(controlled);
        }
        else if (isControlling && !shouldControl)
        {
            StopControl();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Magnet"))
        {
            canControl = true;
            controlled = other.gameObject;
            Debug.Log($"Entered magnetic field of {controlled.name}");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Magnet"))
        {
            canControl = false;
            Debug.Log($"Exited magnetic field of {controlled?.name}");

            // Stop control if the object leaves the field
            if (isControlling)
            {
                StopControl();
            }
        }
    }

    private void StartControl()
    {
        isControlling = true;
        shouldControl = true;
        abilityTimer = abilityDuration; // Initialize the timer

        defaultColor = controlled.GetComponent<SpriteRenderer>().color;

        // Start looping magnetic control sound
        if (audioManager != null && !isMagneticSoundPlaying)
        {
            audioManager.StartMagneticAbilitySound();
            isMagneticSoundPlaying = true;
            Debug.Log("Started magnetic control sound.");
        }

        Debug.Log($"Started controlling {controlled.name} for {abilityDuration} seconds.");
    }

    public void Control(GameObject other)
    {
        Rigidbody2D other_rb = other.GetComponent<Rigidbody2D>();
        Vector2 targetVelocity = new Vector2(horizontalInput * moveSpeed, verticalInput * moveSpeed);
        other_rb.velocity = Vector2.SmoothDamp(other_rb.velocity, targetVelocity, ref moveVelocity, moveTime);
        other_rb.gravityScale = 0f;

        other.GetComponent<SpriteRenderer>().color = highlightColor;

        mv.canMove = false;
        mv.canJump = false;
        mv.isBusy = true;

        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        camera.FollowTemporaryTarget(other);
    }

    public void StopControl()
    {
        if (controlled != null)
        {
            Rigidbody2D other_rb = controlled.GetComponent<Rigidbody2D>();
            other_rb.velocity = Vector2.zero;
            other_rb.gravityScale = 1f;

            controlled.GetComponent<SpriteRenderer>().color = defaultColor;
        }

        mv.canMove = true;
        mv.canJump = true;
        mv.isBusy = false;

        rb.bodyType = RigidbodyType2D.Dynamic;

        isControlling = false;
        shouldControl = false;

        camera.FollowPlayer();

        // Stop looping magnetic control sound
        if (audioManager != null && isMagneticSoundPlaying)
        {
            audioManager.StopMagneticAbilitySound();
            isMagneticSoundPlaying = false;
            Debug.Log("Stopped magnetic control sound.");
        }

        Debug.Log($"Stopped controlling {controlled?.name}");
    }
}
