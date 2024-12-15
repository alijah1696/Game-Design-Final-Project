using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    public Animator animator; // Assign this in the Inspector

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float move = Input.GetAxis("Horizontal");
        Vector3 scale = Vector3.one;

        animator.SetFloat("Speed", Mathf.Abs(move));
        Debug.Log("Character Speed: " + move);

        transform.localScale = scale;
    }
}
