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
    MoveCharacter mv;
    Rigidbody2D rb;

    public float moveSpeed = 1f;
   

    GameObject controlled;
    // Start is called before the first frame update
    void Start()
    {
        mv = GetComponent<MoveCharacter>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)){
            if(canControl && !isControlling){
                isControlling = true;
                shouldControl = true;
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
        other_rb.gravityScale = 0;
        Vector2 targetVelocity = new Vector2(horizontalInput * moveSpeed, verticalInput * moveSpeed);
        other_rb.velocity = targetVelocity;
        other_rb.bodyType = RigidbodyType2D.Kinematic;

        mv.canMove = false;
        mv.canJump = false;
    }

    public void StopControl(){   
        Rigidbody2D other_rb = controlled.GetComponent<Rigidbody2D>();
        other_rb.gravityScale = 1f;        
        Vector2 targetVelocity = new Vector2(0, 0);
        other_rb.velocity = targetVelocity;
        other_rb.bodyType = RigidbodyType2D.Dynamic;

        mv.canMove = true;
        mv.canJump = true;
        
        
        isControlling = false;
        shouldControl = false;
    }
}
