using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacter : MonoBehaviour
{
    // Movement variables
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private bool facingRight = true;
    private bool isGrounded = true; // To check if character is on the ground

    private float horizontalValue;
    public float linearDragX = 0.1f;
    public bool canMove = true;

    private Rigidbody2D rb;
    public List<Rigidbody2D> RigidBodies = new List<Rigidbody2D>();

    private Vector2 movement;
    private Vector2 velocity = Vector2.zero; // Used for SmoothDamp

    private AudioManager audioManager;  // Reference to AudioManager

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Find AudioManager in the scene
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Update()
    {
        // Get horizontal input
        horizontalValue = Input.GetAxisRaw("Horizontal");
        // Check for jump input
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        if (canMove) Move(horizontalValue);
        ApplyXAxisDrag();
    }

    //WALKING METHODS

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

    //JUMPING METHODS

    public void Jump()
    {
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        isGrounded = false; // Character is airborne after jumping
    }

    public void Dash(float x, float y)
    {
        float yMulti = y + 0.25f;
        float xMulti = x * 1.25f;
        rb.AddForce(new Vector2(xMulti, yMulti) * jumpForce * 0.75f, ForceMode2D.Impulse);
        isGrounded = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the character lands on the ground
        if (collision.gameObject.CompareTag("Ground") && !isGrounded)
        {
            isGrounded = true;

            // Play landing sound effect
            audioManager.PlaySFX(audioManager.FloorTouch);
            Debug.Log("Landed on the ground");
        }

        // Check if the character collides with a wall
        if (collision.gameObject.CompareTag("Wall"))
        {
            audioManager.PlaySFX(audioManager.WallTouch);
            Debug.Log("Touched a wall");
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

    public void ResetJump()
    {
        isGrounded = true;
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
        if ((other.transform.localScale).x < 0)
            currentScale.x = Mathf.Abs(currentScale.x) * -1f;
        else
            currentScale.x = Mathf.Abs(currentScale.x);
        transform.localScale = currentScale;
    }

    public bool getDirection()
    {
        return facingRight;
    }
}
