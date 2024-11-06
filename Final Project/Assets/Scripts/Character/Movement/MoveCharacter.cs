using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacter : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private bool facingRight = true;
    private bool isGrounded = true;
    private bool isClimbing = false;

    private float horizontalValue;
    public float linearDragX = 0.1f;
    public bool canMove = true;

    private Rigidbody2D rb;
    public List<Rigidbody2D> RigidBodies = new List<Rigidbody2D>();

    private AudioManager audioManager;  // Reference to AudioManager

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Update()
    {
        horizontalValue = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isClimbing)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        if (canMove && !isClimbing) Move(horizontalValue);
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

        if (audioManager != null && audioManager.jumpSound != null)
        {
            audioManager.PlaySFX(audioManager.jumpSound);
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
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            if (audioManager != null && audioManager.FloorTouch != null)
            {
                audioManager.PlaySFX(audioManager.FloorTouch);
            }
        }
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Vine"))
        {
            isClimbing = true;
            rb.velocity = Vector2.zero;
            if (audioManager != null && audioManager.climbVine != null)
            {
                audioManager.PlaySFX(audioManager.climbVine);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Vine"))
        {
            isClimbing = false;
            if (audioManager != null)
            {
                audioManager.StopClimbingSound();
            }
        }
    }

    void OnDisable()
    {
        if (rb != null)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    public void transferVariablesFrom(MoveCharacter other)
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x = Mathf.Abs(currentScale.x) * (other.transform.localScale.x < 0 ? -1f : 1f);
        transform.localScale = currentScale;
    }

    public bool getDirection()
    {
        return facingRight;
    }
}
