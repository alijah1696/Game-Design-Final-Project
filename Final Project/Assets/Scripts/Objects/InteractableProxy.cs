using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableProxy : MonoBehaviour

{
    public GameObject puzzleInteractable;
    private float progress;

    public bool isFlipped;


    // Start is called before the first frame update
    void Start()
    {
        progress = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Hamster wheel (-1 to 1)
        HamsterWheel hm = puzzleInteractable.GetComponent<HamsterWheel>();
        if(hm != null){
             progress = (hm.getProgress() * 0.5f) + 0.5f;
             return;
        }

        //Button is binary  (0 or 1)
        ButtonLogic bl = puzzleInteractable.GetComponent<ButtonLogic>();
        if(bl != null){
            progress = bl.getProgress();
            return;
        }

        //Pressure plate is analog (0 to 1)
        PressurePlateLogic pl = puzzleInteractable.GetComponent<PressurePlateLogic>();
        if(pl != null){
            progress = pl.GetProgress();
            return;
        }
    }


    //Always returns a number from 0 to 1 
    //if flipped will return 1 to 0
    public float getProgress(){
        if(isFlipped) return (1f - progress);
        else return progress;
    }
}
