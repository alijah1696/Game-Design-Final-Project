using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLogic : MonoBehaviour
{

    private InteractableProxy proxy;
    private Collider2D c2d;
    private SpriteRenderer sr;

    public bool isOpen;

    [SerializeField]
    private Sprite openedSprite;
    private Sprite closedSprite;

    // Start is called before the first frame update
    void Start()
    {
        proxy = GetComponent<InteractableProxy>();
        sr = GetComponent<SpriteRenderer>();
        c2d = GetComponent<Collider2D>();

        closedSprite = sr.sprite;
    }

    // Update is called once per frame
    void Update()
    {   
        float progress = proxy.getProgress();
        
        if(progress == 1 && !isOpen){
            Open();  
        }else if(progress == 0 && isOpen){
            Close();   
        }
    }

    public void Open(){
        isOpen = true;
        c2d.isTrigger = true;
        sr.sprite = openedSprite;
    }

    public void Close(){
        isOpen = false;
        c2d.isTrigger = false;
        sr.sprite = closedSprite;
    }
}
