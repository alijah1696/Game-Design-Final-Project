using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantVineMovement : MonoBehaviour
{

    public float climbSpeed = 5f;
    private bool isClimbing = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float vertical = Input.GetAxis("Vertical");

        if (isClimbing)
        {
            rb.velocity = new Vector2(rb.velocity.x , vertical * climbSpeed);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Vine"))
        {
            isClimbing = true;
            rb.gravityScale = 0; // Disable gravity while climbing
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Vine"))
        {
            isClimbing = false;
            rb.gravityScale = 1; // Re-enable gravity when not climbing
        }
    }
}
