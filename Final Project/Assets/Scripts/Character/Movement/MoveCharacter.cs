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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get horizontal input
        horizontalValue = Input.GetAxisRaw("Horizontal");        
        // Check for jump input
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && isGrounded)
        {
            Jump();
        } else if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)){
            Fall();
        }
    }

    void FixedUpdate()
    {
        if(canMove) Move(horizontalValue);
        ApplyXAxisDrag();
    }   
    
    //WALKING METHODS

    void Move(float dir)
    {
        // Calculate the new position based on input direction and movement speed
        float moveAmount = dir * moveSpeed * Time.deltaTime;

        // Update the position
        transform.position = new Vector3(transform.position.x + moveAmount, transform.position.y, transform.position.z);

        // Flip character based on movement direction
        FlipCharacter(dir);
    }
    

    void ApplyXAxisDrag()
    {
        // Apply drag to the x-axis velocity only
        Vector2 velocity = rb.velocity;

        // Calculate new x velocity by applying drag
        velocity.x = Mathf.Lerp(velocity.x, 0, linearDragX * Time.fixedDeltaTime);

        // Set the Rigidbody's velocity with the modified x component
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
        // Apply jump force
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                 
        isGrounded = false; // Character is airborne after jumping
    }

    public void Fall()
    {
        // Apply jump force
        rb.AddForce(new Vector2(0, -3f), ForceMode2D.Impulse);
                 
        isGrounded = false; // Character is airborne after jumping
    }

    public void Dash(float x, float y){

        float yMulti = y + 0.25f;
        float xMulti = x * 1.25f;
        rb.AddForce(new Vector2(xMulti, yMulti) * jumpForce * 0.75f, ForceMode2D.Impulse);

        isGrounded = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the character lands on the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {   
        // Check if the character lands on the ground
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

    public void ResetJump(){
        isGrounded = true;
    }

    //HELPER METHODS

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
