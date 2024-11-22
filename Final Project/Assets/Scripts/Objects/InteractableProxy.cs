using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableProxy : MonoBehaviour

{
    public GameObject puzzleInteractable;
    private float progress;


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

        ButtonLogic bl = puzzleInteractable.GetComponent<ButtonLogic>();
        if(bl != null){
            progress = bl.getProgress();
            return;
        }
    }

    public float getProgress(){
        return progress;
    }
}
