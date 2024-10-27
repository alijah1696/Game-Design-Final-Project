using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Character_Movement : MonoBehaviour
{
    public Sprite[] sprites; // Array to store multiple sprites
    private SpriteRenderer spriteRenderer;
    private int currentSpriteIndex = 0; //Index to track the current sprite

    private bool isJumping = false;
    private float jumpForce = 10.0f;
    private float gravity = -15f;
    private float verticalVelocity = 0.0f;
    private Vector3 initialPosition;

    public float speed = 5.0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (sprites.Length > 0)
        {
            spriteRenderer.sprite = sprites[currentSpriteIndex]; // Set the initial sprite
        }
        initialPosition = transform.position;
    }

    void Update()
    {
        // Switch to the next sprite in the array when Enter is pressed
        if (Input.GetKeyDown(KeyCode.Return) && sprites.Length > 0)
        {
            currentSpriteIndex = (currentSpriteIndex + 1) % sprites.Length;
            spriteRenderer.sprite = sprites[currentSpriteIndex];
        }

        // Movement
        Vector3 movement = Vector3.zero;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            movement.x -= 1;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            movement.x += 1;

        transform.Translate(movement * speed * Time.deltaTime);

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            isJumping = true;
            verticalVelocity = jumpForce;
        }

        if (isJumping)
        {
            verticalVelocity += gravity * Time.deltaTime;
            transform.Translate(Vector3.up * verticalVelocity * Time.deltaTime);

            if (transform.position.y <= initialPosition.y)
            {
                transform.position = new Vector3(transform.position.x, initialPosition.y, transform.position.z);
                isJumping = false;
                verticalVelocity = 0;
            }
        }
    }
}
