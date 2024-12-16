using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsMove : MonoBehaviour{

    public Rigidbody2D rb2D;
    public Vector2 forceVector;
    public float moveRate = 100f;
    public float startXpos = -171f;
    public float endXpos = 171f;

    public bool isTiling = false;

    void Start(){
        rb2D = gameObject.GetComponent<Rigidbody2D>();

    }

    void FixedUpdate(){
        float moveForce = moveRate * Time.fixedDeltaTime;
        forceVector = new Vector2(moveForce, 0);
        rb2D.velocity = forceVector;

        if (isTiling) {
            if (transform.position.x >= endXpos) {
                //transform.localPosition.x = startXpos;
                Vector2 newPos = new Vector2 (startXpos, transform.position.y);
                transform.position = newPos;
            }
        }
    }


}