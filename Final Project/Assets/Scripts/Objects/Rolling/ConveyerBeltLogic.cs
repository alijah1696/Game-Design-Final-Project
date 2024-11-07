using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyerBeltLogic : MonoBehaviour
{

    public float speed = 1f;
    public bool moveRight = true;

    private float defaultJumpForce;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other){
        GameObject player = other.gameObject;
        MoveCharacter mv = player.GetComponent<MoveCharacter>();

        if(mv != null){
            defaultJumpForce = mv.jumpForce;
            mv.jumpForce = defaultJumpForce/5f;
        }
    }
    
    void OnCollisionStay2D(Collision2D other){
        GameObject gm = other.gameObject;
        Rigidbody2D rb = gm.GetComponent<Rigidbody2D>();

        if(rb != null){
            float pushSpeed = speed * (moveRight ? 1 : -1);
            rb.velocity = new Vector2(pushSpeed, rb.velocity.y);
        }
    }

    void OnCollisionExit2D(Collision2D other){
        GameObject player = other.gameObject;
        MoveCharacter mv = player.GetComponent<MoveCharacter>();

        if(mv != null){
            mv.jumpForce = defaultJumpForce;
        }
    }
}
