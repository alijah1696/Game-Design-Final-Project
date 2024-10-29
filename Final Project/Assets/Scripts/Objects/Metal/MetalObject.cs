using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalObject : MonoBehaviour
{   
    private bool pushable = false;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pushable)
        {
            rb.velocity = new Vector2(0, 0);
            rb.isKinematic = true; // Make the Rigidbody2D kinematic
        }
        else
        {
            rb.isKinematic = false; // Make the Rigidbody2D dynamic
        }
    }

    public void Push()
    {
        pushable = true;
    }
    
    public void StopPush()
    {
        pushable = false;
    }
}
