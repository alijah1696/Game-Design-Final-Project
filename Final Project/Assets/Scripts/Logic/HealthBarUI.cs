using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{   
    [SerializeField]
    private GameObject[] icons;
    [SerializeField] private Color dimmedColor;
    [SerializeField] private float lerpSpeed;

    [SerializeField] private Color plantColor;
    [SerializeField] private Color robotColor;
    private Color goalColor;

    private SwapCharacters sc;
 
    // Start is called before the first frame update
    void Start()
    {
        sc = FindObjectOfType<SwapCharacters>();
        SetUp();
    }
    
    // Update is called once per frame
    void Update()
    {   
        HandleHearts();
    }

    void SetUp(){
        goalColor = plantColor;
        for(int i = 0; i < icons.Length; i++){
            icons[i].GetComponent<Image>().color = goalColor;
        }
    }



    void HandleHearts(){
        goalColor = sc.IsPlantActive() ? plantColor : robotColor;

        for(int i = 0; i < icons.Length; i++){
            Image image = icons[i].GetComponent<Image>();

            if((i+1) > sc.CurrentLives()){
                image.color = Color.Lerp(image.color, dimmedColor, lerpSpeed);
            }else{
                image.color = Color.Lerp(image.color, goalColor, lerpSpeed);
            }
        }
    }

  
    
}
