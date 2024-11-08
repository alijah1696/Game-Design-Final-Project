using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageticAbilities : MonoBehaviour
{   

    private bool canControl;
    private bool isControlling;
    private bool shouldControl;

    private float verticalInput;
    private float horizontalInput;

    CameraFollow camera;
    MoveCharacter mv;
    Rigidbody2D rb;

    public float moveSpeed = 1f;
    public float moveTime = 0.1f;
    private Vector2 moveVelocity = Vector2.zero;
    

    public Color highlightColor;
    private Color defaultColor;
   

    GameObject controlled;
    // Start is called before the first frame update
    void Start()
    {
        mv = GetComponent<MoveCharacter>();
        rb = GetComponent<Rigidbody2D>();
        camera = FindObjectOfType<CameraFollow>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)){
            if(canControl && !isControlling){
                isControlling = true;
                shouldControl = true;

                defaultColor = controlled.GetComponent<SpriteRenderer>().color;
            }
            else if(isControlling) shouldControl = false;
        }

        verticalInput = Input.GetAxisRaw("Vertical");
        horizontalInput = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate(){
        if(isControlling && shouldControl) Control(controlled);
        else if(isControlling && !shouldControl) StopControl();
    }


    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Magnet")){
            canControl = true;
            controlled = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("Magnet")){
            canControl = false;
        }
    }

    public void Control(GameObject other){
        Rigidbody2D other_rb = other.GetComponent<Rigidbody2D>();
        Vector2 targetVelocity = new Vector2(horizontalInput * moveSpeed, verticalInput * moveSpeed);
        other_rb.velocity = Vector2.SmoothDamp(other_rb.velocity, targetVelocity, ref moveVelocity, moveTime);
        other_rb.gravityScale = 0f;

        other.GetComponent<SpriteRenderer>().color = highlightColor;

        mv.canMove = false;
        mv.canJump = false;

        rb.velocity = new Vector2(0, 0);
        rb.bodyType = RigidbodyType2D.Kinematic;

        camera.FollowTemporaryTarget(other);
    }

    public void StopControl(){   
        Rigidbody2D other_rb = controlled.GetComponent<Rigidbody2D>();       
        Vector2 targetVelocity = new Vector2(0, 0);
        other_rb.velocity = targetVelocity;
        other_rb.gravityScale = 1f;

        controlled.GetComponent<SpriteRenderer>().color = defaultColor;

        mv.canMove = true;
        mv.canJump = true;

        rb.bodyType = RigidbodyType2D.Dynamic;
        
        isControlling = false;
        shouldControl = false;

        camera.FollowPlayer();
    }
}
