using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantVineMovement : MonoBehaviour
{
    public float climbSpeed = 5f;
    private bool isClimbing = false;
    private Rigidbody2D rb;
    private MoveCharacter mv;
    private float defaultGravityScale;
    
    private GameObject grapplePoint;
    private bool isGrappling = false;
    private bool externallyCalled = false;
    private SpringJoint2D sj;
    private float radiusToDistanceMultiplier = 4.3f;
    [SerializeField] private float distanceToAnimSpeedMultiplier;
    private float defualtMoveSpeed;
    private float goalMoveSpeed;
    [SerializeField] private float moveSpeedIncrease;
    [SerializeField] private float swingClimbSpeed;
    private float swingClimbProgress;

    [SerializeField] private float jumpMulti;

    [SerializeField] private TrailRenderer trail;
    [SerializeField] private float radiusToTrailMultiplier;
   
    [SerializeField] private GameObject vineRenderer;
    private VineGrappleAnimation va;
    [SerializeField] private float lerpSpeed;


    private AudioManager audioManager;  // Reference to AudioManager

    private float angleSpin = 0f; // Declare and initialize angleSpin

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sj = GetComponent<SpringJoint2D>();
        va = vineRenderer.GetComponent<VineGrappleAnimation>();
        mv = GetComponent<MoveCharacter>();

        defaultGravityScale = rb.gravityScale;
        defualtMoveSpeed = mv.moveSpeed;

        // Find AudioManager in the scene
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        HandleClimbing();
        HandleGrappling();
        ChangeMoveSpeed();
        AdjustTrail();
    }

    void HandleClimbing()
    {
        if (isClimbing)
        {   
            mv.StartAbility();
            float vertical = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(0, vertical * climbSpeed);
        }
        if(!isClimbing && !isGrappling) mv.EndAbility();
    }

    void HandleGrappling()
    {
        if (Input.GetKeyDown(KeyCode.E) || externallyCalled)
        {
            bool animDone = va.AnimationDone();
            bool fullyExtended = va.FullyExtended();

            if (!isGrappling && grapplePoint != null && animDone && !(grapplePoint.GetComponent<VineGrappleLogic>().OnCooldown()) && !externallyCalled)
            {
                float currentDist = DistanceTo(grapplePoint);
                float speedMulti = currentDist / distanceToAnimSpeedMultiplier;

                // Play grapple sound
                audioManager?.PlayGrappleSound(); // Call AudioManager to play the grapple sound

                //Set animation Variables
                va.SetBroken(false);
                StartCoroutine(va.AnimateRope(grapplePoint.transform.position, 1f * speedMulti));

                // Set Rigid Body variables
                StartCoroutine(LerpVariables());
                rb.velocity = new Vector2(rb.velocity.x, 0);
                sj.connectedBody = grapplePoint.GetComponent<Rigidbody2D>();
                isGrappling = true;
                sj.enabled = true;

                mv.StartAbility();
            }
            else if (isGrappling && (fullyExtended || externallyCalled))
            {
                va.SetBroken(true);

                //cooldown
                VineGrappleLogic vgl = grapplePoint.GetComponent<VineGrappleLogic>();
                vgl.Cooldown();

                // Set Rigid Body variables
                isGrappling = false;
                sj.connectedBody = null;
                sj.enabled = false;

                mv.EndAbility();
            }
            vineRenderer.SetActive(true);
            externallyCalled = false;
        }
    }

    public void EndSwing(){
        externallyCalled = true;
    }

    IEnumerator LerpVariables(){
        

        float vineRadius = (grapplePoint.GetComponent<CircleCollider2D>().radius);
        float vineDistance = vineRadius * (2f/3f) * radiusToDistanceMultiplier;
        float speedMulti = DistanceTo(grapplePoint) / distanceToAnimSpeedMultiplier;

        float percent = 0;

        bool particlePlayed = false;
        VineGrappleLogic vgl = grapplePoint.GetComponent<VineGrappleLogic>();
        while(percent < 1 && grapplePoint != null){
            percent += Time.deltaTime * lerpSpeed / speedMulti; 
            
            //change distance of joint
            sj.distance = Mathf.Lerp(DistanceTo(grapplePoint), vineDistance, percent);
            //change move speed of characters
            goalMoveSpeed = defualtMoveSpeed + moveSpeedIncrease * (DistanceTo(grapplePoint) / distanceToAnimSpeedMultiplier);
            //play animation
            if(!particlePlayed && va.Progress() > 0.85){
                particlePlayed = true;
                vgl.Particle();
            }

            yield return null;
        }
        StartCoroutine(ChangeVineHeight(vineDistance, 1f/3f));
        while(isGrappling && grapplePoint != null){
            bool abovePoint = (transform.position.y - grapplePoint.transform.position.y) > 0;

            if(abovePoint){
               sj.frequency = 2f;
            }else{
                sj.frequency = 5f;
            }  
            yield return null;
        }
        EndJump();
    }

    IEnumerator ChangeVineHeight(float midDistance, float increment)
    {
        swingClimbProgress = 0.5f;

        increment *= (grapplePoint.GetComponent<CircleCollider2D>().radius) * radiusToDistanceMultiplier;
        float lowDistance = midDistance + increment;
        float highDistance = midDistance - increment;

        while (isGrappling && grapplePoint != null)
        {
            float dir = Input.GetAxisRaw("Vertical");

            // Update swing climb progress
            float oldDistance = sj.distance; // Store the old distance
            swingClimbProgress += dir * Time.deltaTime * swingClimbSpeed;
            swingClimbProgress = Mathf.Clamp01(swingClimbProgress);

            // Update the joint distance
            sj.distance = Mathf.Lerp(lowDistance, highDistance, swingClimbProgress);

            // Check if the distance changed and play the sound
            if (Mathf.Abs(sj.distance - oldDistance) > 0.01f) // Small threshold to avoid rapid triggers
            {
                audioManager?.PlayGrappleSound();
            }

            // Update move speed dynamically
            goalMoveSpeed = defualtMoveSpeed + moveSpeedIncrease * (DistanceTo(grapplePoint) / distanceToAnimSpeedMultiplier);
            yield return null;
        }
    }

    void EndJump(){
        float force = goalMoveSpeed/(jumpMulti * defualtMoveSpeed);
        mv.Dash(0, force);
    }

    void ChangeMoveSpeed(){
        float progress = va.Progress();
        
        if(progress > 0){
            mv.moveSpeed = Mathf.Lerp(defualtMoveSpeed, goalMoveSpeed, progress);
        }
    }
    
    void AdjustTrail(){
        bool notValid = !isGrappling || grapplePoint == null;
        float goalTime = notValid ? 0 : (DistanceTo(grapplePoint)/radiusToDistanceMultiplier) * radiusToTrailMultiplier;
        float lerpMulti = notValid  ? 1.5f : 1f;
        trail.time = Mathf.Lerp(trail.time, goalTime, (lerpSpeed/lerpMulti) * Time.deltaTime);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Vine"))
        {
            isClimbing = true;
            rb.gravityScale = 0; // Disable gravity while climbing

            // Play a random climbing sound
            audioManager?.PlayRandomClimbingSound();
        }
    }
    
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("VineGrapple") && !isGrappling && grapplePoint != other.gameObject)
        {
            bool oldCooldown = grapplePoint != null ? grapplePoint.GetComponent<VineGrappleLogic>().OnCooldown() : true;
            bool newCooldown = other.gameObject.GetComponent<VineGrappleLogic>().OnCooldown();

            if(oldCooldown && !newCooldown){
                grapplePoint = other.gameObject;
            }else if(!oldCooldown && !newCooldown){
                float oldDistance = grapplePoint != null ? DistanceTo(grapplePoint) : float.MaxValue;
                float newDistance = DistanceTo(other.gameObject);
                
                if (newDistance < oldDistance)
                {   
                    grapplePoint = other.gameObject;
                }
            }
        }
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
            rb.gravityScale = defaultGravityScale; // Re-enable gravity when not climbing

            // Stop the climbing sound
            audioManager?.StopClimbingSound();
        }

        if (other.CompareTag("VineGrapple") && !isGrappling)
        {
            grapplePoint = null;
        }
    }

    float DistanceTo(GameObject other)
    {   
        if(other == null) return 0;
        return Vector2.Distance(transform.position, other.transform.position);
    }

    public bool IsGrappling(){
        return isGrappling;
    }

    public bool IsClimbing(){
        return isClimbing;
    }
}
