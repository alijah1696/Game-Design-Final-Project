using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicGas : MonoBehaviour
{
    public GameObject respawnPoint;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {   
        if (other.CompareTag("Plant"))
        {
            // Teleport the object to the respawn point
            
            other.transform.position = respawnPoint.transform.position;
            other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2();
        }
    }
}
