using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetPush : MonoBehaviour
{
    private Rigidbody2D rb;
    public float pushSpeed = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Magnet"))
        {   
            MetalObject mo = collision.gameObject.GetComponent<MetalObject>();
            if(mo != null) mo.Push();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Magnet"))
        {   
            MetalObject mo = collision.gameObject.GetComponent<MetalObject>();
            if(mo != null) mo.StopPush();
        }
    }
}
