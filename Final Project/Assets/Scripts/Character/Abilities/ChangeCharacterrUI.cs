using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCharacterrUI : MonoBehaviour
{
    private bool isOnCooldown = false;  // Flag to check if the coroutine is on cooldown
    [SerializeField]
    private float cooldownTime;  // Cooldown duration in seconds
    SwapCharacters sw;

    [SerializeField]
    private RectTransform group;
    private Vector3 originalGroupPos;

    [SerializeField]
    private GameObject plantIcon;
    [SerializeField]
    private GameObject robotIcon;
    [SerializeField]
    private GameObject center;

    
    private float wrongProgress;
    private bool wrongCalled;
    private float wrongAngle;
    [SerializeField] private int wrongAnimSpin;
    [SerializeField] private float wrongAnimDist;

    private Vector3 originalPlantScale;
    private Vector3 originalRobotScale;

    private Color originalColor = Color.white;
    [SerializeField]
    private Color dimmedColor;
    private Color originalOutlineColor;

    [SerializeField]
    private Color greenColor;
    [SerializeField]
    private Color blueColor;
    [SerializeField]
    private GameObject background;
    [SerializeField]
    private GameObject outline;
    
    

    private Image plantImage;
    private Image robotImage;
    private Image outlineImage;
    private Image backgroundImage;
    
    [SerializeField]
    private float radius;
    [SerializeField]
    private float angleOffset;

    [SerializeField]
    private float largeMulti;
    [SerializeField]
    private float smallMulti;


    private bool isPlant = true;
    private float angleSpin = 0f;
    
    void Start(){
        originalPlantScale = plantIcon.transform.localScale;
        originalRobotScale = robotIcon.transform.localScale;
    
        plantImage = plantIcon.GetComponent<Image>();
        robotImage = robotIcon.GetComponent<Image>();
        backgroundImage = background.GetComponent<Image>();
        outlineImage = outline.GetComponent<Image>();

        originalOutlineColor = outlineImage.color;

        sw = FindObjectOfType<SwapCharacters>();

        originalGroupPos = group.GetComponent<RectTransform>().localPosition;
    }

    void Update()
    {   
        CorrectAbilityAnimation();
        WrongAbilityAnimation();
        
        if (Input.GetButtonDown("SwapCharacter") || Input.GetKeyDown(KeyCode.Tab))
        {
            Button_SwapCharacer();
        }
    }

    public void Button_SwapCharacer(){
        if (!isOnCooldown){
            if(sw.IsNotBusy()){
                StartCoroutine(AbilityCoroutine());
            }else{
                StartCoroutine(WrongAbilityCoroutine());
            }
        }
    }

    private void CorrectAbilityAnimation(){
                //angle progs goes from 0 to 1
        float angleProgress = (Mathf.Abs(Mathf.Sin(angleSpin/2 * Mathf.Deg2Rad)));

        //Change Positions
        Vector3 centerPos = center.transform.position;

        Vector2 RobotCharOffset = new Vector2(Mathf.Cos((angleSpin + angleOffset) * Mathf.Deg2Rad + ((180f) * Mathf.Deg2Rad)), Mathf.Sin((angleSpin + angleOffset) * Mathf.Deg2Rad + (180f * Mathf.Deg2Rad))) * radius;
        Vector2 PlantCharOffset = new Vector2(Mathf.Cos((angleSpin + angleOffset) * Mathf.Deg2Rad), Mathf.Sin((angleSpin + angleOffset) * Mathf.Deg2Rad)) * radius;

        plantIcon.transform.position = centerPos + (Vector3)PlantCharOffset;
        robotIcon.transform.position = centerPos + (Vector3)RobotCharOffset;

        //Change Scale
        float robotScaleMulti = Mathf.Lerp(smallMulti, largeMulti, angleProgress);
        float plantScaleMulti = Mathf.Lerp(largeMulti, smallMulti, angleProgress);

        robotIcon.transform.localScale = originalRobotScale * robotScaleMulti;
        plantIcon.transform.localScale = originalPlantScale * plantScaleMulti;


        //Change Color and Alpha
        if(!wrongCalled){
            plantImage.color = Color.Lerp(originalColor, dimmedColor, angleProgress);
            robotImage.color = Color.Lerp(dimmedColor, originalColor, angleProgress);
            backgroundImage.color = Color.Lerp(greenColor, blueColor, angleProgress);
            outlineImage.color = originalOutlineColor;
        }else{
            //plantImage.color = Color.Lerp(plantImage.color, dimmedColor, wrongProgress);
            //robotImage.color = Color.Lerp(robotImage.color, dimmedColor, wrongProgress);
            //outlineImage.color = Color.Lerp(originalOutlineColor, Color.grey, wrongProgress);
        }
    }
    

    private IEnumerator AbilityCoroutine()
    {
        isOnCooldown = true;
        isPlant = !isPlant;

        // Adjust angleSpin based on character switch
        float goalAngle = angleSpin + 180f;

        LeanTween.value(gameObject, UpdateAngle, angleSpin, goalAngle, cooldownTime - 0.1f).setEase(LeanTweenType.easeInOutCirc);
        
        
        sw.SwapCharacter();

        // Wait for the cooldown time
        yield return new WaitForSeconds(cooldownTime);
        isOnCooldown = false;
    }

    void UpdateAngle(float a){
        angleSpin = a;
    }

    private IEnumerator WrongAbilityCoroutine(){
        isOnCooldown = true;
        wrongCalled = true;

        float cd = cooldownTime/2;
        float cdSlow = 2;

        wrongAngle = Random.Range(0, 361);


        LeanTween.value(gameObject, UpdateWrongProgress, 0, 1, cd/cdSlow).setEase(LeanTweenType.easeInSine);
        yield return new WaitForSeconds(cd/cdSlow);
        
        LeanTween.value(gameObject, UpdateWrongProgress, 1, 0, cd/cdSlow).setEase(LeanTweenType.easeInSine);
        yield return new WaitForSeconds(cd/cdSlow);

        wrongCalled = false;
        isOnCooldown = false;
    }

    void UpdateWrongProgress(float p){
        wrongProgress = p;
    }

    void WrongAbilityAnimation(){
        float angleOffset = Mathf.Sin((wrongProgress * wrongAnimSpin * Mathf.PI) + (Mathf.Deg2Rad * wrongAngle));
        
        float vOffset = wrongAnimDist * angleOffset;
        float hOffset = wrongAnimDist * angleOffset;

        Vector3 newPos = originalGroupPos + new Vector3(hOffset, vOffset, 0f);
        group.GetComponent<RectTransform>().localPosition = newPos;
    }
}
