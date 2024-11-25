using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCharacterUI : MonoBehaviour
{
    public GameObject[] sprites;
    public GameObject icons;

    private Color[] originalColors;

    public GameObject center;
    private Vector3[] spriteOffsets;
    
    public GameObject backSprite;
    private Vector3 backScale;
    public GameObject overlaySprite;
    private Vector3 overlayScale;

    private float charIndex = 1;
    private CharacterPickDirection direction;


    private CanvasGroup canvasGroup;
    private bool open;
    private float lerpSpeed = 5f;
    public float timeScaleCutOff = 0.6f;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = icons.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;

        overlayScale = overlaySprite.transform.localScale;
        backScale = backSprite.transform.localScale;

        overlaySprite.transform.localScale = new Vector3();
        backSprite.transform.localScale = new Vector3();

        spriteOffsets = new Vector3[sprites.Length];
        originalColors = new Color[sprites.Length];
        for (int i = 0; i < sprites.Length; i++)
        {
            spriteOffsets[i] = new Vector3(); // Create a new transform
            spriteOffsets[i] = sprites[i].transform.position - center.transform.position; // Store the original position
            sprites[i].transform.position = center.transform.position;

            originalColors[i] = new Color();
            originalColors[i] = sprites[i].GetComponent<Image>().color;
        }
    }

    // Update is called once per frame
    void Update()
    {   
        SwapCharacters sc = FindObjectOfType<SwapCharacters>();
        if(!sc.isCurrentlyBusy()){
            if (Input.GetKey(KeyCode.LeftShift)){
                ShowUI();
                if(Time.timeScale <= timeScaleCutOff){
                    GetDirection();
                    GetCharIndex();
                    ChooseCharacter(); 
                }
            }else{
                HideUI();
                if(Time.timeScale > timeScaleCutOff){
                    SwapToCharacter();
                }
            }
            
            if(Time.timeScale != 0){
                Time.timeScale = Mathf.Lerp(1f, 0.01f,canvasGroup.alpha);
            }
        }
        HandleAnimations();
    }

    void HandleAnimations(){
        float progress = canvasGroup.alpha;
        float lerpedTime = lerpSpeed * Time.unscaledDeltaTime;

        for(int i = 0; i < sprites.Length; i++){
            CanvasGroup cv = sprites[i].GetComponent<CanvasGroup>();
            
            Vector3 centerPos = center.transform.position;
            Vector3 offsetPos = spriteOffsets[i] * progress;
            if(i != charIndex){
                sprites[i].transform.position = Vector3.Lerp(sprites[i].transform.position, centerPos + offsetPos, lerpedTime * 2);
                cv.ignoreParentGroups = false;
                sprites[i].GetComponent<Image>().color = Color.Lerp(sprites[i].GetComponent<Image>().color, Color.grey, lerpedTime);
            }else{
                sprites[i].transform.position = Vector3.Lerp(sprites[i].transform.position, centerPos, lerpedTime);
                cv.ignoreParentGroups = true;
                sprites[i].GetComponent<Image>().color = Color.Lerp(sprites[i].GetComponent<Image>().color, originalColors[i], lerpedTime);
            }
            
        }

        overlaySprite.transform.localScale = Vector3.Lerp(overlaySprite.transform.localScale, overlayScale * progress, lerpedTime * 2);
        backSprite.transform.localScale = Vector3.Lerp(backSprite.transform.localScale, backScale * progress, lerpedTime * 2);
    }
    
    void ChooseCharacter(){
        for(int i = 0; i < sprites.Length; i++){
            CanvasGroup cg = sprites[i].GetComponent<CanvasGroup>();
            cg.alpha = (i == charIndex) ? 1f : 0.5f;
        }
    }

    void SwapToCharacter(){
        SwapCharacters sc = FindObjectOfType<SwapCharacters>();
        if(sc == null) return;
        if(sc.GetCurrentIndex() != charIndex){
            float cIndex = sc.GetCurrentIndex();

            if(charIndex == 0) sc.SetPlantForm2();
            if(charIndex == 1) sc.SetPlantForm1();
            if(charIndex == 2) sc.SetPlantForm3();
            if(charIndex == 3) sc.SetRobotForm3();
            if(charIndex == 4) sc.SetRobotForm1();
            if(charIndex == 5) sc.SetRobotForm2();

            if(cIndex >=  3 && charIndex < 3) sc.SwapCharacter();
            else if(cIndex <  3 && charIndex >= 3) sc.SwapCharacter();
        }
    }

    void GetCharIndex(){
        if(direction == CharacterPickDirection.NorthWest) charIndex = 0;
        if(direction == CharacterPickDirection.North) charIndex = 1;
        if(direction == CharacterPickDirection.NorthEast) charIndex = 2;
        if(direction == CharacterPickDirection.SouthEast) charIndex = 3;
        if(direction == CharacterPickDirection.South) charIndex = 4;
        if(direction == CharacterPickDirection.SouthWest) charIndex = 5;
    }

    void ShowUI(){
        open = true;
        canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1f, lerpSpeed * Time.unscaledDeltaTime);
    }

    void HideUI(){
        canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0f, lerpSpeed * Time.unscaledDeltaTime);
        if (canvasGroup.alpha <= 0.01f){
            open = false;
            canvasGroup.alpha = 0f;
        }
    }

    public bool isOpen(){
        return open;
    }

    void GetDirection()
    {
        float hDirection = Input.GetAxisRaw("Horizontal");
        float vDirection = Input.GetAxisRaw("Vertical");

        // West is (-1, 0)
        if (vDirection == 0 && hDirection == -1)
            direction = CharacterPickDirection.West;

        // NorthWest is (-1, 1)
        else if (vDirection == 1 && hDirection == -1)
            direction = CharacterPickDirection.NorthWest;

        // North is (0, 1)
        else if (vDirection == 1 && hDirection == 0)
            direction = CharacterPickDirection.North;

        // NorthEast is (1, 1)
        else if (vDirection == 1 && hDirection == 1)
            direction = CharacterPickDirection.NorthEast;

        // East is (1, 0)
        else if (vDirection == 0 && hDirection == 1)
            direction = CharacterPickDirection.East;

        // SouthEast is (-1, 1)
        else if (vDirection == -1 && hDirection == 1)
            direction = CharacterPickDirection.SouthEast;

        // South is (0, -1)
        else if (vDirection == -1 && hDirection == 0)
            direction = CharacterPickDirection.South;

        // SouthWest is (-1, -1)
        else if (vDirection == -1 && hDirection == -1)
            direction = CharacterPickDirection.SouthWest;
    }


}



public enum CharacterPickDirection
{
    West,
    NorthWest,
    North,
    NorthEast,
    East,
    SouthEast,
    South,
    SouthWest
}
