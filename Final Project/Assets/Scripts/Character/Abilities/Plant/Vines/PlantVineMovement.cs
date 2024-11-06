using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantVineMovement : MonoBehaviour
{
    public float climbSpeed = 5f;
    private bool isClimbing = false;
    private Rigidbody2D rb;
    private float defaultGravityScale;

    public float grappleForce = 7f;
    private bool canGrapple = false;
    private bool isGrappling = false;

    private GameObject grapplePoint;
    public float grappleRange = 1.5f; // Range within which the player will stay still

    private AudioManager audioManager;  // Reference to AudioManager

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultGravityScale = rb.gravityScale;

        // Find AudioManager in the scene
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Update()
    {
        float vertical = Input.GetAxis("Vertical");
        if (isClimbing)
        {
            rb.velocity = new Vector2(0, vertical * climbSpeed);
        }

        if (Input.GetKeyDown(KeyCode.E) && canGrapple)
        {
            Grapple(grapplePoint);
        }

        MoveCharacter mv = GetComponent<MoveCharacter>();

        if (isGrappling && grapplePoint != null && Vector2.Distance(transform.position, grapplePoint.transform.position) <= grappleRange)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.gravityScale = defaultGravityScale; // Re-enable gravity when space is pressed
                mv.Dash(Input.GetAxisRaw("Horizontal"), 1f);
                isGrappling = false;
            }
            else
            {
                rb.velocity = Vector2.zero; // Keep the player stationary
                rb.gravityScale = 0; // Disable gravity while stationary
            }
        }
        mv.canMove = !isGrappling;
    }

    public void Grapple(GameObject point)
    {
        if (point != null)
        {
            Vector2 directionToGrapple = ((Vector2)point.transform.position - (Vector2)transform.position).normalized;
            float distance = Vector2.Distance(transform.position, point.transform.position);
            Vector2 grappleVector = directionToGrapple * distance * grappleForce;

            rb.velocity = Vector2.zero;
            rb.gravityScale = defaultGravityScale * 0.5f;
            rb.AddForce(grappleVector, ForceMode2D.Impulse);

            // Play grapple sound effect if you have a specific sound for grappling
            if (audioManager != null && audioManager.climbVine != null)
            {
                audioManager.PlaySFX(audioManager.climbVine);  // Change this to a specific grapple sound if available
            }

            isGrappling = true;
            canGrapple = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Vine"))
        {
            isClimbing = true;
            rb.gravityScale = 0; // Disable gravity while climbing

            // Start the climbing sound
            if (audioManager != null)
            {
                audioManager.StartClimbingSound();
            }
        }
        else if (other.CompareTag("VineGrapple"))
        {
            grapplePoint = other.gameObject;
            canGrapple = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("VineGrapple"))
        {
            grapplePoint = other.gameObject;
            canGrapple = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Vine"))
        {
            isClimbing = false;
            rb.gravityScale = defaultGravityScale; // Re-enable gravity when not climbing

            // Stop the climbing sound
            if (audioManager != null)
            {
                audioManager.StopClimbingSound();
            }
        }
        else if (other.CompareTag("VineGrapple"))
        {
            canGrapple = false;
            grapplePoint = null;
        }
    }
}
