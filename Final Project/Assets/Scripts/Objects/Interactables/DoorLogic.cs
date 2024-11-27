using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLogic : MonoBehaviour
{

    private InteractableProxy proxy;
    private Collider2D c2d;
    
    private SpriteRenderer sr;
    public bool isOpen;
    public Color offColor;
    private Color onColor;
    // Start is called before the first frame update
    void Start()
    {
        proxy = GetComponent<InteractableProxy>();
        sr = GetComponent<SpriteRenderer>();
        c2d = GetComponent<Collider2D>();

        onColor = sr.color;
    }

    // Update is called once per frame
    void Update()
    {
        float progress = proxy.getProgress();
        if(progress == 1){
            Opened();
            isOpen = true;
        }else if(progress == 0){
            Closed();
            isOpen = false;
        }
    }

    void Opened(){
        sr.color = offColor;
        c2d.isTrigger = true;
    }

    void Closed(){
        sr.color = onColor;
        c2d.isTrigger = false;
    }
}
