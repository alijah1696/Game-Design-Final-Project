using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public SpriteRenderer idle;
    public SpriteRenderer rolled;

    private bool isIdleActive = true;
    
    private float rollAngle;
    public float rollAngleSpeed = 1f;
    
    private MoveCharacter mv;
    private Rigidbody2D rb;
    private float horizontalValue;
    
    private float baseSpeed;
    public float speedMultiplier = 1.5f;
    private float maxSpeed;
    public float speedIncrease = 1f;
    private float speedProgesss;
    
    private float baseJump;
    public float jumpMulitplier = 1f;
    


    void Start()
    {
        // Ensure only one sprite renderer is active at the start
        idle.enabled = isIdleActive;
        rolled.enabled = !isIdleActive;
        
        //variables
        mv = GetComponent<MoveCharacter>();
        rb = GetComponent<Rigidbody2D>();
        baseSpeed = mv.moveSpeed;
        baseJump = mv.jumpForce;

        
        maxSpeed = baseSpeed * speedMultiplier;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SwapMode();
        }


        horizontalValue = Input.GetAxisRaw("Horizontal");
        rolled.transform.rotation = Quaternion.Euler(0, 0, rollAngle);
        
    }

    void FixedUpdate(){
        if(!isIdleActive) Roll(horizontalValue);
    }


    //Movement Methods
    void Roll(float dir){
        rollAngle += rollAngleSpeed * mv.moveSpeed * dir * -0.1f;
        
        if((mv.GetDirection() && dir < 0) || (!mv.GetDirection() && dir > 0)){
            speedProgesss = 0;
        }else if(dir != 0){
            speedProgesss = Mathf.Lerp(0f, 1f, speedProgesss + (speedIncrease * (0.01f) ));
        }else if(dir == 0){
            speedProgesss = 0;
        }

        mv.moveSpeed = Mathf.Lerp(baseSpeed/2, maxSpeed, speedProgesss);
    }


    //Logic methods
    void BallMode(){
    }

    void IdleMode(){
    }

    void SwapMode()
    {
        isIdleActive = !isIdleActive;
        idle.enabled = isIdleActive;
        rolled.enabled = !isIdleActive;

        mv.moveSpeed = baseSpeed;
        mv.jumpForce = baseJump;


        speedProgesss = 0f;

        if(isIdleActive) IdleMode();
        else BallMode();
    }

    //Helper methods
    public float GetRollAngle(){
        return rollAngle;
    }

    public bool IsRolled(){
        return !isIdleActive;
    }
}
