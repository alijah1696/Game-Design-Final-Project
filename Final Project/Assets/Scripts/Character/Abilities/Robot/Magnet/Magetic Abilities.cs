using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticAbilities : MonoBehaviour
{
    [Header("Ability Settings")]
    [Tooltip("Set the duration (in seconds) for how long the magnetic ability lasts.")]
    [Min(0.1f)] // Ensures duration is always positive
    public float abilityDuration = 999f; // Duration of the ability (editable in the Inspector)
    private float abilityTimer = 0f;   // Tracks remaining time for the ability

    private bool canControl;          // Indicates if a magnet object can be controlled
    private bool isControlling;       // Indicates if the ability is currently active
    private bool shouldControl;       // Indicates if control should continue

    private float verticalInput;
    private float horizontalInput;

    private CameraFollow camera;
    private MoveCharacter mv;
    private Rigidbody2D rb;

    private AudioManager audioManager; // Reference to AudioManager

    public float moveSpeed = 1f;
    private float moveSpeedMulti;
    [SerializeField]
    private float moveSpeedFactor;
    public float moveTime = 0.1f;
    private Vector2 moveVelocity = Vector2.zero;

    public Color highlightColor;
    private Color defaultColor;

    private GameObject controlled;       // The object being controlled
    private bool isMagneticSoundPlaying; // Tracks if the magnetic sound is playing

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
            // Start controlling the object if one was previously detected
            if (controlled != null && !isControlling && canControl)
            {
                StartControl();
            }
            else if (isControlling)
            {
                shouldControl = false;
            }
        }

        CalculateSpeed();

        verticalInput = Input.GetAxisRaw("Vertical");
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Handle the ability timer
        if (isControlling)
        {
            abilityTimer -= Time.deltaTime;

            if (abilityTimer <= 0f)
            {
                shouldControl = false;
                StopControl();
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

    void OnTriggerStay2D(Collider2D other){
        if (other.CompareTag("Magnet"))
        {
            if(!isControlling){
                float oldDist = controlled != null ? DistanceTo(controlled) : float.MaxValue;
                float newDist = DistanceTo(other.gameObject);

                if(newDist < oldDist){
                    controlled = other.gameObject;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Magnet"))
        {
            controlled = other.gameObject;
            if(!isControlling) canControl = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Magnet"))
        {
            if(other.gameObject == controlled && !isControlling){
                canControl = false;
                controlled = null;
            }
        }
    }

    float DistanceTo(GameObject other)
    {
        return Vector2.Distance(transform.position, other.transform.position);
    }

    private void StartControl()
    {
        if (controlled == null) return;

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

    public bool IsControlling(){
        return isControlling;
    }

    public GameObject GetControlled(){
        return controlled;
    }

    void CalculateSpeed(){
        moveSpeedMulti = moveSpeedFactor/ObjectScaleFactor(controlled);
    }

    float ObjectScaleFactor(GameObject g)
    {
        if(g == null) return 0;

        Vector3 scale = g.transform.localScale;
        return (scale.x + scale.y)/2f;
    }

    public void Control(GameObject other)
    {
        if (other == null) return;

        Rigidbody2D other_rb = other.GetComponent<Rigidbody2D>();
        Vector2 targetVelocity = new Vector2(horizontalInput * moveSpeed * moveSpeedMulti, verticalInput * moveSpeed  * moveSpeedMulti);
        other_rb.velocity = Vector2.SmoothDamp(other_rb.velocity, targetVelocity, ref moveVelocity, moveTime);
        other_rb.gravityScale = 0f;

        //other.GetComponent<SpriteRenderer>().color = highlightColor;

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

        controlled = null;

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
