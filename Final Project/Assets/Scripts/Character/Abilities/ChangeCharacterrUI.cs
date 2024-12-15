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
    private GameObject plantIcon;
    [SerializeField]
    private GameObject robotIcon;
    [SerializeField]
    private GameObject center;

    private Vector3 originalPlantScale;
    private Vector3 originalRobotScale;

    private Color originalColor = Color.white;
    [SerializeField]
    private Color dimmedColor;

    [SerializeField]
    private Color greenColor;
    [SerializeField]
    private Color blueColor;
    [SerializeField]
    private GameObject background;
    private Image backgroundImage;
    

    private Image plantImage;
    private Image robotImage;
    
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

        sw = FindObjectOfType<SwapCharacters>();
    }

    void Update()
    {   
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
        plantImage.color = Color.Lerp(originalColor, dimmedColor, angleProgress);
        robotImage.color = Color.Lerp(dimmedColor, originalColor, angleProgress);
        backgroundImage.color = Color.Lerp(greenColor, blueColor, angleProgress);
        
        Debug.Log(sw.IsNotBusy());
        if (Input.GetKeyDown(KeyCode.Tab) && !isOnCooldown && sw.IsNotBusy())
        {
            StartCoroutine(AbilityCoroutine());
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
}
