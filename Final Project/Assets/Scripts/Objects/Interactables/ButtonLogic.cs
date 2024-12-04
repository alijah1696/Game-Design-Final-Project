using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLogic : MonoBehaviour
{

    private float progress;
    public bool pressed;
    private bool canPress;
    
    private SpriteRenderer sr;
    private Sprite unpressedSprite;
    [SerializeField]
    private Sprite pressedSprite;

    [SerializeField]
    private GameObject lightSource;
    UnityEngine.Rendering.Universal.Light2D lt;
    private float maxLight;

    [SerializeField]
    private bool forPlant;  


    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if(lightSource != null) lt = lightSource.GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        if(lt != null) maxLight = lt.intensity;
        if(sr != null) unpressedSprite = sr.sprite;
    }

    // Update is called once per frame
    void Update()
    {   
        if(canPress && Input.GetKeyDown(KeyCode.E)) Press();
    }

    void Press(){
        if(sr == null) return; 
        
        pressed = !pressed;
        if(pressed){
            sr.sprite = pressedSprite;
            if(lt != null) lt.intensity = 0;
            progress = 1;
        }else{
            sr.sprite = unpressedSprite;
            if(lt != null) lt.intensity = maxLight;
            progress = 0;
        } 
        
    }


    void OnTriggerEnter2D(Collider2D  other){
        bool isValid = 
        ((forPlant && other.CompareTag("Plant")) ||
        (!forPlant && other.CompareTag("Robot")));

        if(isValid) canPress = true;
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
