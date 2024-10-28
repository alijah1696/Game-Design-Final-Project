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
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        Move(horizontalValue);
    }

    void Move(float dir)
    {
        float xVal = dir * moveSpeed * 100 * Time.deltaTime;
        Vector2 targetVelocity = new Vector2(xVal, rb.velocity.y);
        rb.velocity = targetVelocity;

        // Flip character based on movement direction
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

    void Jump()
    {
        // Apply jump force
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        isGrounded = false; // Character is airborne after jumping
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the character lands on the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnDisable()
    {
        // Ensure the character stops moving horizontally when swapped out
        if (rb != null)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    // Method to add to transfer variables on swap
    public void transferVariablesFrom(MoveCharacter other)
    {
        // Arrange position to match
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
