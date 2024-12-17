using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicGas : MonoBehaviour
{
    public GameObject respawnPoint;
    SwapCharacters sc;

    void Start()
    {
        sc = FindObjectOfType<SwapCharacters>();
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {   
        if (other.CompareTag("Plant"))
        {   
            sc.Kill(respawnPoint);
        }
        else if (other.CompareTag("Robot"))
        {
            MoveCharacter mv = sc.GetCurrentForm().GetComponent<MoveCharacter>();
            mv.InDanger();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {   
        if (other.CompareTag("Robot"))
        {
            MoveCharacter mv = sc.GetCurrentForm().GetComponent<MoveCharacter>();
            mv.Safe();
        }
    }
}
