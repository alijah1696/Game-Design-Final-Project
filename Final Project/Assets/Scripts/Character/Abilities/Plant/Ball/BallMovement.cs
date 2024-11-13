using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public SpriteRenderer idle;
    public SpriteRenderer rolled;

    private bool isIdleActive = true; // Indicates if the ball is in idle or rolling mode
    private float rollAngle;
    public float rollAngleSpeed = 1f;

    private MoveCharacter mv;
    private Rigidbody2D rb;
    private float horizontalValue;

    private float baseSpeed;
    public float speedMultiplier = 1.5f;
    private float maxSpeed;
    public float speedIncrease = 1f;
    private float speedProgress;

    private float baseJump;
    public float jumpMultiplier = 1f;

    private AudioManager audioManager; // Reference to AudioManager
    private bool isRollingSoundPlaying = false; // Track if the rolling sound is currently playing

    void Start()
    {
        // Initialize sprites and variables
        idle.enabled = isIdleActive;
        rolled.enabled = !isIdleActive;

        mv = GetComponent<MoveCharacter>();
        rb = GetComponent<Rigidbody2D>();
        baseSpeed = mv.moveSpeed;
        baseJump = mv.jumpForce;
        maxSpeed = baseSpeed * speedMultiplier;

        // Find AudioManager in the scene
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SwapMode(); // Swap between idle and ball mode when "E" is pressed
        }

        horizontalValue = Input.GetAxisRaw("Horizontal");
        rolled.transform.rotation = Quaternion.Euler(0, 0, rollAngle); // Rotate the ball based on movement
    }

    void FixedUpdate()
    {
        if (!isIdleActive)
        {
            Roll(horizontalValue); // Roll the ball if in ball mode
        }
    }

    // Movement method for rolling
    void Roll(float dir)
    {
        rollAngle += rollAngleSpeed * mv.moveSpeed * dir * -0.1f;

        if ((mv.GetDirection() && dir < 0) || (!mv.GetDirection() && dir > 0))
        {
            speedProgress = 0;
        }
        else if (dir != 0)
        {
            speedProgress = Mathf.Lerp(0f, 1f, speedProgress + (speedIncrease * 0.01f));
        }
        else if (dir == 0)
        {
            speedProgress = 0;
        }

        mv.moveSpeed = Mathf.Lerp(baseSpeed / 2, maxSpeed, speedProgress);

        // Start rolling sound if moving, stop if idle
        if (audioManager != null)
        {
            if (dir != 0 && !isRollingSoundPlaying)
            {
                audioManager.StartBallMovementSound();
                isRollingSoundPlaying = true;
                Debug.Log("Starting ball movement sound: Direction = " + dir);
            }
            else if (dir == 0 && isRollingSoundPlaying)
            {
                audioManager.StopBallMovementSound();
                isRollingSoundPlaying = false;
                Debug.Log("Stopping ball movement sound: Direction = " + dir);
            }
        }
    }

    // Activate ball mode, starting the sound if moving
    void BallMode()
    {
        Debug.Log("Entering Ball Mode.");

        // Start ball movement sound when switching to Ball Mode
        if (audioManager != null && horizontalValue != 0)
        {
            audioManager.StartBallMovementSound();
            isRollingSoundPlaying = true;
            Debug.Log("Ball movement sound started.");
        }
    }

    // Activate idle mode, stopping the sound
    void IdleMode()
    {
        Debug.Log("Entering Idle Mode.");

        // Stop ball movement sound when switching to Idle Mode
        if (audioManager != null)
        {
            audioManager.StopBallMovementSound();
            isRollingSoundPlaying = false;
            Debug.Log("Ball movement sound stopped.");
        }
    }

    // Swap between Idle and Ball Mode
    void SwapMode()
    {
        isIdleActive = !isIdleActive;
        idle.enabled = isIdleActive;
        rolled.enabled = !isIdleActive;

        mv.moveSpeed = baseSpeed;
        mv.jumpForce = baseJump;
        speedProgress = 0f;

        if (isIdleActive)
        {
            IdleMode(); // Switch to idle mode
        }
        else
        {
            BallMode(); // Switch to ball mode
        }

        Debug.Log("Swapped mode: isIdleActive = " + isIdleActive);
    }

    // Helper method to get the current roll angle
    public float GetRollAngle()
    {
        return rollAngle;
    }

    // Check if the object is in rolled (ball) mode
    public bool IsRolled()
    {
        return !isIdleActive;
    }
}
