using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BG_Parallax : MonoBehaviour
{

    private float length, startposX;
    //startposY;
    public GameObject cam;
    public float parallaxEffect;
    //parallaxUpEffect;

    void Start()
    {
        startposX = transform.position.x;
        //startposY = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float distX = (cam.transform.position.x * parallaxEffect);
        //float distY = (cam.transform.position.y * parallaxUpEffect);
        transform.position = new Vector3(startposX + distX, transform.position.y, transform.position.z);
        //transform.position = new Vector3(startposX + distX, startposY + distY, 1);
        if (temp > startposX + length)
        {
            startposX += length;
        }
        else if (temp < startposX - length)
        {
            startposX -= length;
        }
    }

}
