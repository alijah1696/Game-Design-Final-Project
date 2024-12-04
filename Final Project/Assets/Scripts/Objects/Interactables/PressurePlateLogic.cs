using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateLogic : MonoBehaviour
{

    public GameObject platePivot;
    private Vector3 emptyScale;

    private bool beingPressed;
    public float pressSpeed = 2f;

    float progress;

    private List <GameObject> currentTriggers;

    // Start is called before the first frame update
    void Start()
    {
        emptyScale = platePivot.transform.localScale;
        currentTriggers = new List <GameObject> ();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        beingPressed = currentTriggers.Count > 0;
        
        float lerped = pressSpeed * Time.fixedDeltaTime;
        if(beingPressed){
            progress = Mathf.Lerp(progress, 1, lerped); 
            if(progress > 0.95)progress = 1;
        }else{
            progress = Mathf.Lerp(progress, 0, lerped * 2f); 
            if(progress < 0.05) progress =  0;
        }

        Vector3 pressedScale = new Vector3(emptyScale.x, 0f, emptyScale.z);
        platePivot.transform.localScale = Vector3.Lerp(emptyScale, pressedScale, progress);
    }

    public float GetProgress(){
        return progress;
    }

    void OnTriggerEnter2D(Collider2D other){
        if(IsValid(other.gameObject) && !currentTriggers.Contains(other.gameObject)){
            currentTriggers.Add(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if(IsValid(other.gameObject) && currentTriggers.Contains(other.gameObject)){
            currentTriggers.Remove(other.gameObject);
        }
    }

    bool IsValid(GameObject o){
        return
            o.tag != "Plant" 
         && o.tag != "Robot"
         && o.GetComponent<Rigidbody2D>() != null;
    }

}
