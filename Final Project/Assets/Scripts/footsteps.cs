using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public AudioSource footstepsSound;

    private void Update()
    {
     if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            footstepsSound.enabled = true;

        }
     else
        {
            footstepsSound.enabled = false;
        }
    }
}
