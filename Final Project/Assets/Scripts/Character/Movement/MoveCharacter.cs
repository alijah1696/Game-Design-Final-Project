using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacter : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 15f;
    public bool isGrounded = true;
    public bool canJump = true;

    public bool facingRight = true;
    private float horizontalValue;
    public float linearDragX = 2.5f;
    public bool canMove = true;

    public bool notSlowed = true;
    public bool isBusy = false;
    private float defaultGravityScale;
    public float gravityScaleIncrease = 2.5f;

    private bool usingAbility = false;
    private bool inDanger = false;

    private Rigidbody2D rb;
    private AudioManager audioManager; // Reference to AudioManager    

    private bool isWalkingSoundPlaying = false; // Track if walking sound is currently playing

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Initialize the AudioManager
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();

        defaultGravityScale = rb.gravityScale;
    }

    void Update()
    {
        // Read horizontal input from keyboard or controller (left stick)
        horizontalValue = Input.GetAxis("Horizontal");

        // Ensure character is not slowed due to time scaling
        notSlowed = (Time.timeScale >= 0.4f);

        // Handle jumping with "A" on the controller or "Space" on the keyboard
        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space)) && isGrounded && canJump && notSlowed)
        {
            Jump();
        }

        // Increase gravity when falling
        IncreaseGravity();
    }

    public bool IsMoving()
    {
        return Mathf.Abs(rb.velocity.x) > 0.1f; // Check if horizontal velocity is significant
    }

    void FixedUpdate()
    {
        if (canMove && notSlowed)
        {
            Move(horizontalValue);
            HandleWalkingSound(horizontalValue);
        }
        ApplyXAxisDrag();
    }

    public void Move(float dir)
    {
        float moveAmount = dir * moveSpeed * Time.fixedDeltaTime;
        transform.position = new Vector3(transform.position.x + moveAmount, transform.position.y, transform.position.z);
        FlipCharacter(dir);
    }

    void ApplyXAxisDrag()
    {
        Vector2 velocity = rb.velocity;
        velocity.x = Mathf.Lerp(velocity.x, 0, linearDragX * Time.fixedDeltaTime);
        rb.velocity = velocity;
    }

    void FlipCharacter(float dir)
    {
        Vector3 currentScale = transform.localScale;
        if (facingRight && dir < 0)
        {
            currentScale.x = Mathf.Abs(currentScale.x) * -1f;
            facingRight = false;
        }
        else if (!facingRight && dir > 0)
        {
            currentScale.x = Mathf.Abs(currentScale.x) * 1f;
            facingRight = true;
        }
        transform.localScale = currentScale;
    }

    public void Jump()
    {
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        isGrounded = false;

        // Play jump sound if available
        if (audioManager != null && audioManager.jumpSound != null)
        {
            audioManager.PlaySFX(audioManager.jumpSound);
        }
    }

    private void IncreaseGravity()
    {
        if (canMove)
        {
            // Increases gravity at apex of jump or when moving downward
            if (rb.velocity.y < 0f || Input.GetAxisRaw("Vertical") < 0)
            {
                rb.gravityScale = defaultGravityScale + gravityScaleIncrease;
            }
        }

        // Reset gravity scale when grounded or at zero vertical velocity
        if (isGrounded || rb.velocity.y == 0f)
        {
            rb.gravityScale = defaultGravityScale;
        }
    }

    public void Dash(float x, float y)
    {
        float dashMultiplier = 1.25f;
        rb.AddForce(new Vector2(x * dashMultiplier, y * dashMultiplier) * jumpForce, ForceMode2D.Impulse);
        isGrounded = false;
    }

    public void ResetJump()
    {
        isGrounded = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Magnet"))
        {
            // Ensure walking sound stops when leaving the ground
            if (audioManager != null && isWalkingSoundPlaying)
            {
                audioManager.StopWalkingSound();
                isWalkingSoundPlaying = false;
                Debug.Log("Walking sound stopped (not grounded).");
            }
        }
    }

    void HandleWalkingSound(float direction)
    {
        // Play walking sound if moving and grounded, stop if idle or airborne
        if (audioManager != null && isGrounded)
        {
            if (direction != 0 && !isWalkingSoundPlaying)
            {
                // Start walking sound loop
                audioManager.StartWalkingSound(); // Play FloorTouch as looping sound
                isWalkingSoundPlaying = true;
                Debug.Log("Walking sound started.");
            }
            else if (direction == 0 && isWalkingSoundPlaying)
            {
                // Stop walking sound loop
                audioManager.StopWalkingSound();
                isWalkingSoundPlaying = false;
                Debug.Log("Walking sound stopped.");
            }
        }
    }

    public void TransferVariablesFrom(MoveCharacter other)
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x = Mathf.Abs(currentScale.x) * (other.transform.localScale.x < 0 ? -1f : 1f);
        transform.localScale = currentScale;

        isGrounded = other.isGrounded;
        isBusy = other.isBusy;

        // Flip character
        int dir = other.GetDirection() ? 1 : -1;
        FlipCharacter(dir);
    }

    public bool GetDirection()
    {
        return facingRight;
    }

    public bool IsOnGround()
    {
        return isGrounded;
    }


    //State Methods

    public bool IsBusy(){
        isBusy = inDanger || !isGrounded || usingAbility;
        return isBusy;
    }

    public void InDanger(){
        inDanger = true;
    }
    public void Safe(){
        inDanger = false;
    }
    public void OnGround(){
        isGrounded = true;
    }
    public void InAir(){
        isGrounded = false;
    }
    public void StartAbility(){
        usingAbility = true;
    }
    public void EndAbility(){
        usingAbility = false;
    }

}
