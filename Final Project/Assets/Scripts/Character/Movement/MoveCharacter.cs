using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacter : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 15f;
    private bool isGrounded = true;
    public bool canJump = true;

    private bool facingRight = true;
    private float horizontalValue;
    public float linearDragX = 2.5f;
    public bool canMove = true;

    private float defaultGravityScale;
    public float gravityScaleIncrease = 2.5f;

    private Rigidbody2D rb;
    private AudioManager audioManager;  // Reference to AudioManager

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Initialize the AudioManager
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();

        defaultGravityScale = rb.gravityScale;
    }

    void Update()
    {
        horizontalValue = Input.GetAxisRaw("Horizontal");
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && isGrounded && canJump)
        {
            Jump();
        }
        IncreaseGravity();
    }

    void FixedUpdate()
    {
        if (canMove) Move(horizontalValue);
        ApplyXAxisDrag();
    }

    void Move(float dir)
    {
        float moveAmount = dir * moveSpeed * Time.deltaTime;
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Play sound when touching the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            if (audioManager != null && audioManager.FloorTouch != null)
            {
                audioManager.PlaySFX(audioManager.FloorTouch);
            }
        }

        // Play sound when touching a wall
        if (collision.gameObject.CompareTag("Wall"))
        {
            if (audioManager != null && audioManager.WallTouch != null)
            {
                audioManager.PlaySFX(audioManager.WallTouch);
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void OnDisable()
    {
        if (rb != null)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    public void TransferVariablesFrom(MoveCharacter other)
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x = Mathf.Abs(currentScale.x) * (other.transform.localScale.x < 0 ? -1f : 1f);
        transform.localScale = currentScale;

        isGrounded = other.isGrounded;

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
}
