using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterWheel : MonoBehaviour
{   

    public SpriteRenderer leftCircle;
    public SpriteRenderer rightCircle;
    
    private float leftRotation;
    private float initialLeftRotation;
    private float rightRotation;  
    private float initialRightRotation;  

    private float initialAngle;
    private float addedAngle;

    private float progress;
    public float progressIncrement = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        progress = -1;

    }

    // Update is called once per frame
    void Update()
    {
        leftCircle.transform.rotation = Quaternion.Euler(0, 0, leftRotation);
        rightCircle.transform.rotation = Quaternion.Euler(0, 0, rightRotation);

        
    }

    void OnTriggerEnter2D(Collider2D other){
        GameObject ball = other.gameObject;
        MoveCharacter mv = ball.GetComponent<MoveCharacter>();
        BallMovement bm = ball.GetComponent<BallMovement>();

        if(mv != null && bm != null){
            initialAngle = bm.GetRollAngle();
            initialLeftRotation = leftRotation;
            initialRightRotation = rightRotation;
        }
    }

    void OnTriggerStay2D(Collider2D other){
        GameObject ball = other.gameObject;
        MoveCharacter mv = ball.GetComponent<MoveCharacter>();
        BallMovement bm = ball.GetComponent<BallMovement>();

        if(mv != null && bm != null && bm.IsRolled()){
            addedAngle = bm.GetRollAngle() - initialAngle;

            leftRotation = initialLeftRotation - addedAngle;
            rightRotation = initialRightRotation - addedAngle;

            float increment = (Input.GetAxis("Horizontal") * progressIncrement * 0.01f);
            progress = Mathf.Clamp(progress + increment, -1, 1);
            Debug.Log(progress);
        }
    }

    void OnTriggerExit2D(Collider2D other){
        GameObject ball = other.gameObject;
        MoveCharacter mv = ball.GetComponent<MoveCharacter>();
        BallMovement bm = ball.GetComponent<BallMovement>();

        if(mv != null && bm != null){
            addedAngle = 0f;
        }
    }

    public float getProgress(){
        return progress;
    }
}
