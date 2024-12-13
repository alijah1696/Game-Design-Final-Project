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
            SwapCharacters sc = FindObjectOfType<SwapCharacters>();
            sc.Kill(respawnPoint);
        }
    }
}
