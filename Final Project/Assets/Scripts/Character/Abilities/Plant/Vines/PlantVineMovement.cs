using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantVineMovement : MonoBehaviour
{
    public float climbSpeed = 5f;
    private bool isClimbing = false;
    private Rigidbody2D rb;
    private float defualtGravityScale;

    public float grappleForce = 7f;
    private bool canGrapple = false;    
    private bool isGrapling = false;

    private GameObject grapplePoint;
    public float grappleRange = 1.5f; // Range within which the player will stay still

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defualtGravityScale = rb.gravityScale;
    }

    void Update()
    {
        float vertical = Input.GetAxis("Vertical");
        if (isClimbing)
        {
            rb.velocity = new Vector2(0, vertical * climbSpeed);
        }

        if(Input.GetKeyDown(KeyCode.E) && canGrapple){
            Grapple(grapplePoint);
        }

        MoveCharacter mv = GetComponent<MoveCharacter>();
        
        if (isGrapling && grapplePoint != null && Vector2.Distance(transform.position, grapplePoint.transform.position) <= grappleRange)
        {   
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.gravityScale = defualtGravityScale; // Re-enable gravity when space is pressed
                mv.Dash(Input.GetAxisRaw("Horizontal"), 1f);
                
                isGrapling = false;
                
            }
            else
            {
                rb.velocity = Vector2.zero; // Keep the player stationary
                rb.gravityScale = 0; // Disable gravity while stationary
            }
        }
        mv.canMove = !isGrapling;
    }

    public void Grapple(GameObject point)
    {
        if (point != null)
        {

            // Calculate direction from the current object to the grapple point
            Vector2 directionToGrapple = ((Vector2)point.transform.position - (Vector2)transform.position).normalized;
            
            // Calculate distance to the grapple point
            float distance = Vector2.Distance(transform.position, point.transform.position);
            
            // Calculate force based on distance
            Vector2 grappleVector = directionToGrapple * distance * grappleForce;
            
            // Apply force to the Rigidbody
            rb.velocity = Vector2.zero;
            rb.gravityScale = defualtGravityScale * 0.5f;
            rb.AddForce(grappleVector, ForceMode2D.Impulse);        
            

            MoveCharacter mv = GetComponent<MoveCharacter>();
            isGrapling = true;
            canGrapple = false;
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

    void OnTriggerStay2D(Collider2D other){
        if (other.CompareTag("VineGrapple"))
        {
            grapplePoint = other.gameObject;
            VineGrappleLogic vgLogic = grapplePoint.GetComponent<VineGrappleLogic>();
            if (vgLogic.CanGrapple()) canGrapple = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Vine"))
        {
            isClimbing = false;
            rb.gravityScale = defualtGravityScale; // Re-enable gravity when not climbing
        }

        if (other.CompareTag("VineGrapple"))
        {
            canGrapple = false;
            grapplePoint = null;
        }
    }
}
