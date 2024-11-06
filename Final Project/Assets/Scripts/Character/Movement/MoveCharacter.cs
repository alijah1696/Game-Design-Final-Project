using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacter : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private bool facingRight = true;
    private bool isGrounded = true;
    private float horizontalValue;
    public float linearDragX = 0.1f;
    public bool canMove = true;

    private Rigidbody2D rb;

    private AudioManager audioManager;  // Reference to AudioManager

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioManager = null;
    }

    void Update()
    {
        horizontalValue = Input.GetAxisRaw("Horizontal");
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

        //flip charater
        int dir = other.GetDirection() ? 1 : -1;
        FlipCharacter(dir);
    }

    public bool GetDirection()
    {
        return facingRight;
    }

    public bool IsOnGround(){
        return isGrounded;
    }
    
}
