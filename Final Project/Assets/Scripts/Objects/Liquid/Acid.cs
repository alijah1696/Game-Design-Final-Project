using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acid : MonoBehaviour
{   
    public GameObject respawnPoint;
    SwapCharacters sc;

    // Start is called before the first frame update
    void Start()
    {
        sc = FindObjectOfType<SwapCharacters>();
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {   
        if (other.CompareTag("Plant") || other.CompareTag("Robot"))
        {   
            sc.Kill(respawnPoint);
        }
    }

}
