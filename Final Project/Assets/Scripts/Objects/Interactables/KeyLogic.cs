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

    void Start()
    {
        Debug.Log("KeyLogic: Start method called.");
        sw = FindObjectOfType<SwapCharacters>();
        if (sw == null)
        {
            Debug.LogError("SwapCharacters script not found in the scene!");
        }
    }

    void Update()
    {
        Debug.Log("KeyLogic: Update method called.");
        if (isCollected && playerTransform != null)
        {
            FollowPlayer();
        }
        if (isCollected && ShouldNotFollow())
        {
            Debug.Log("KeyLogic: Key stopped following the player.");
            playerTransform = null;
            isCollected = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"KeyLogic: Collision detected with {collision.gameObject.name}.");
        if (forPlant && collision.CompareTag("Plant"))
        {
            Debug.Log("KeyLogic: Collected by Plant.");
            CollectKey(collision.transform);
        }
        else if (!forPlant && collision.CompareTag("Robot"))
        {
            Debug.Log("KeyLogic: Collected by Robot.");
            CollectKey(collision.transform);
        }
    }

    void CollectKey(Transform player)
    {
        Debug.Log($"KeyLogic: Key collected by {player.name}.");
        isCollected = true;
        playerTransform = player;
    }

    void FollowPlayer()
    {
        offset.x = Mathf.Abs(offset.x) * (sw.IsFacingRight() ? -1 : 1);

        Vector3 targetPosition = playerTransform.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        Debug.Log($"KeyLogic: Following player to {targetPosition}.");
    }

    public void Unlock(Transform goal)
    {
        isCollected = false;
        GetComponent<Collider2D>().enabled = false;

        Vector3 targetPosition = goal.position;
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * 0.5f * Time.deltaTime);

        if (!startedUnlock)
        {
            Debug.Log("KeyLogic: Unlock animation started.");
            LeanTween.rotate(gameObject, new Vector3(0, 0, 90), 1f).setEase(LeanTweenType.easeOutQuint);
            startedUnlock = true;
        }

        unlockProgress = Vector3.Distance(transform.position, targetPosition) < 0.1 ? 1 : 0;
    }

    public float GetUnlockProgress()
    {
        Debug.Log($"KeyLogic: Unlock progress = {unlockProgress}.");
        return unlockProgress;
    }

    bool ShouldNotFollow()
    {
        bool shouldNotFollow = (forPlant && !sw.IsPlantActive()) || (!forPlant && sw.IsPlantActive());
        Debug.Log($"KeyLogic: ShouldNotFollow = {shouldNotFollow}.");
        return shouldNotFollow;
    }
}
