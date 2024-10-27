using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacter : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get input for horizontal movement (A/D or Arrow Keys)
        movement.x = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        // Preserve vertical velocity (e.g., for gravity) while setting horizontal velocity
        rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);
    }

}
