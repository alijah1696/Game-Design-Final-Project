using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLogic : MonoBehaviour
{

    public GameObject buttonPivot;
    public SpriteRenderer buttonSprite; 

    public Color offColor;
    private Color onColor;

    public float offHeight;
    private Vector3 onScale;

    private float progress;
    public bool pressed;
    private bool canPress;


    // Start is called before the first frame update
    void Start()
    {
        onColor = buttonSprite.color;
        onScale = buttonPivot.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {   
        if(canPress && Input.GetKeyDown(KeyCode.E)) Press();
    }

    void Press(){
        Vector3 offScale = new Vector3(onScale.x, offHeight, onScale.z);
        pressed = !pressed;

        if(pressed){
            buttonSprite.color = offColor;
            buttonPivot.transform.localScale = offScale;
            progress = 1;
        }else{
            buttonSprite.color = onColor;
            buttonPivot.transform.localScale = onScale;
            progress = 0;
        }
        
    }


    void OnTriggerEnter2D(Collider2D  other){
        if(isPlayer(other.gameObject))
        canPress = true;
    }

    void OnTriggerExit2D(Collider2D  other){

        if(isPlayer(other.gameObject))
        canPress = false;
    }

    public float getProgress(){
        return progress;
    }

    private bool isPlayer(GameObject other){
        return other.GetComponent<MoveCharacter>() != null;
    }

}
