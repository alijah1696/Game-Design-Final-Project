using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneManager : MonoBehaviour
{

    [SerializeField] private float CameraIncrease;
    [SerializeField] private Vector3 CamaraOffsetIncrease;
    [SerializeField] private float cameraSpeedIncrease;
    private Vector3 cameraOffset;
    private float cameraSize;
    private float cameraSpeed;
    private CameraFollow camera;
    private Camera cm;

    [SerializeField] private GameObject sun;
    [SerializeField] private Transform sunEndPos;
    [SerializeField] private float sunUpTime;
    private Vector3 sunStartPos;    
    private float sunProgress;

    [SerializeField] private List<Light2D> globalLights;
    private List<float> endLights;
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor; 

    private bool startCredits;
    private float creditProgess;   
    [SerializeField]
    private float creditTime; 
    [SerializeField] private float endcreditDiff;
    [SerializeField] private RectTransform creditCanvas;

    [SerializeField] private CanvasGroup swap;


    [SerializeField] private float maxDistance;
    private Vector3 startDistance;
    private float progress;

    [SerializeField] private float speedDampen;
    private float lerpSpeed;

    private bool started;
    private GameObject player;
    private MoveCharacter mc;



    // Start is called before the first frame update
    void Start()
    {
        camera = FindObjectOfType<CameraFollow>();
        cameraOffset = camera.offset;
        cm = camera.gameObject.GetComponent<Camera>();
        cameraSize = cm.orthographicSize;
        cameraSpeed = camera.followSpeed;

        endLights = new List<float>(new float[globalLights.Count]);

        for(int i = 0; i < globalLights.Count; i++){
            endLights[i] = globalLights[i].intensity;
        }
    }

    void FixedUpdate(){
        if(mc != null){
            mc.Move(1f);
            mc.facingRight = true;
        }
    }


    // Update is called once per frame
    void Update()
    {   
        progress = GetProgress();
        Debug.Log(progress);
        if(creditProgess ==  0){
            HandleColors();
        }
        if(started){
            HandleCamera();
            HandleSun();
            HandleCredits();
        }
    } 

    void HandleCredits(){
        if(sunProgress == 1 && progress == 1 && creditProgess == 0){
            StartCoroutine(StartCreditScene());
        }
        
        
    }
    
    IEnumerator StartCreditScene(){
        creditProgess = 0.01f;
        yield return new WaitForSeconds(2);
        LeanTween.value(gameObject, UpdateCreditProgress, 0.01f, 3f, creditTime).setEase(LeanTweenType.easeInOutSine);

        Vector3 startPosition = creditCanvas.position;
        Vector3 endPosition = startPosition + new Vector3(0, endcreditDiff, 0);

        while(creditProgess <= 1){
            cm.backgroundColor = Color.Lerp(endColor, Color.white, creditProgess);
            yield return null;
        }
        while(creditProgess < 3){
            if(creditProgess < 2) yield return null; 
            creditCanvas.position = Vector3.Lerp(startPosition, endPosition, (creditProgess - 2f));
            yield return null;
        }

        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene ("EndMenu");
    }

    void UpdateCreditProgress(float p){
        creditProgess = p;
    }


    void HandleSun(){
        if(sunProgress == 0){
            StartCoroutine(UpdateSunPos());
            sunStartPos = sun.transform.position;
        }

        sun.transform.position = Vector3.Lerp(sunStartPos, sunEndPos.position, sunProgress);
    }

    void HandleColors(){
        float cutOff  = 0.15f;

        

        if(sunProgress > cutOff){
            float cutProgress = (sunProgress - cutOff)/(1f - cutOff);   
            cm.backgroundColor = Color.Lerp(startColor, endColor, cutProgress);

            swap.alpha = Mathf.Lerp(1, 0f, cutProgress * 2f);
            
            for(int i = 0; i < globalLights.Count; i++){
                globalLights[i].intensity = Mathf.Lerp((endLights[i] - 0.5f) , endLights[i], cutProgress);
            }
        }else{
            cm.backgroundColor = startColor;
            for(int i = 0; i < globalLights.Count; i++){
                globalLights[i].intensity = endLights[i] - 0.5f;
            }
        }   
    }
    

    IEnumerator UpdateSunPos(){
        LeanTween.value(gameObject, UpdateSunProgress, 0, 1f, sunUpTime).setEase(LeanTweenType.easeInOutSine);
        yield return null;
    }

    void UpdateSunProgress(float p){
        sunProgress = p;
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.GetComponent<MoveCharacter>() != null){
            started = true;
            startDistance = other.gameObject.transform.position;
            player = other.gameObject;
            mc = player.GetComponent<MoveCharacter>();
            mc.InDanger();
            mc.canMove = false;
            mc.canJump = false;
        }
    }

    void HandleCamera(){
        cm.orthographicSize = Mathf.Lerp(cameraSize, (cameraSize + CameraIncrease), progress);
        camera.offset = Vector3.Lerp(cameraOffset, (cameraOffset + CamaraOffsetIncrease), progress);

        float cutOff  = 0.25f;
        if(sunProgress > cutOff){
            float camprogress = ((sunProgress - cutOff) / (1 - cutOff)) * progress * 1.15f;
            camera.followSpeed = Mathf.Lerp((cameraSpeed + cameraSpeedIncrease), cameraSpeed * 5f, camprogress);
            camera.FollowTemporaryTarget(sun);
        }
    }

    float GetProgress(){

        if(started){
            Vector3 endDistance = startDistance + new Vector3(maxDistance, 0f, 0f);
            float difference = Mathf.Abs(player.transform.position.x - startDistance.x);
            float p = Mathf.Clamp((difference/maxDistance), 0, 1);
            
            return p;
        }else{
            return 0;
        }
    }
}
