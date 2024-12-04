using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyLogic : MonoBehaviour
{
    private bool isCollected = false;
    private Transform playerTransform;
    private float followSpeed = 5f;

    private float unlockProgress;
    private bool startedUnlock;
    
    
    [SerializeField]
    private Vector3 offset;

    private SwapCharacters sw;

    [SerializeField] private bool forPlant;


    void Start(){
        sw = FindObjectOfType<SwapCharacters>();
    }

    // Update is called once per frame
    void Update()
    {   
        if (isCollected && playerTransform != null)
        {
            FollowPlayer();
        }
        if(isCollected && ShouldNotFollow()){
            playerTransform = null;
            isCollected = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (forPlant && collision.CompareTag("Plant"))
        {
            CollectKey(collision.transform);
        }
        else if (!forPlant && collision.CompareTag("Robot"))
        {
            CollectKey(collision.transform);
        }
    }

    void CollectKey(Transform player)
    {
        isCollected = true;
        playerTransform = player;
    }

    void FollowPlayer()
    {
        // Move the key towards the player's position
        offset.x = Mathf.Abs(offset.x) * (sw.IsFacingRight() ? -1 : 1);

        Vector3 targetPosition = playerTransform.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    } 

    public void Unlock(Transform goal){
        isCollected = false;
        GetComponent<Collider2D>().enabled = false;

        Vector3 targetPosition = goal.position;
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * 0.5f * Time.deltaTime);
        
        //Quaternion goalRotation = Quaternion.Euler(0, 0, 90);
        //transform.rotation = Quaternion.Lerp(transform.localRotation, goalRotation, followSpeed * 2f * Time.deltaTime);

        if(!startedUnlock){
            LeanTween.rotate(gameObject, new Vector3(0, 0, 90), 1f).setEase(LeanTweenType.easeOutQuint);
            startedUnlock = true;
        }
        
        unlockProgress =  Vector3.Distance(transform.position, targetPosition) < 0.1 ? 1 : 0;
    }

    public float GetUnlockProgress(){
        return unlockProgress;
    }

    
    bool ShouldNotFollow(){
        return (
        (forPlant && !sw.IsPlantActive()) || 
        (!forPlant && sw.IsPlantActive())
        );
    }

}

