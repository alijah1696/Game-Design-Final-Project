using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacter : MonoBehaviour
{   
    //movement
    public float moveSpeed = 5f;
    
    //direction
    private bool facingRight = true;

    private float horizontalValue;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontalValue = Input.GetAxisRaw("Horizontal");
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

        Vector3 currentScale = transform.localScale;
        if(facingRight && dir < 0){
            currentScale.x = currentScale.x * -1f;
            facingRight = false;
        }
        else if(!facingRight && dir > 0){
            currentScale.x = currentScale.x * -1f;
            facingRight = true;
        }
        transform.localScale = currentScale;
    }

    void OnDisable()
    {
        // Ensure the character stops moving horizontally when swapped out
        if(rb != null){
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    
    }

}
