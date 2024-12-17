using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water: MonoBehaviour
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
        if (other.CompareTag("Robot"))
        {   
            sc.Kill(respawnPoint);
        }
        else if (other.CompareTag("Plant"))
        {
            MoveCharacter mv = sc.GetCurrentForm().GetComponent<MoveCharacter>();
            mv.InDanger();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {   
        if (other.CompareTag("Plant"))
        {
            MoveCharacter mv = sc.GetCurrentForm().GetComponent<MoveCharacter>();
            mv.Safe();
        }
    }
}
