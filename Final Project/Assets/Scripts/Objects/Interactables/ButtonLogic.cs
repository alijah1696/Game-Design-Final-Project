using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLogic : MonoBehaviour
{
    [Header("Button State")]
    private float progress;
    public bool pressed;
    private bool canPress;

    private SpriteRenderer sr;
    private Sprite unpressedSprite;
    [SerializeField]
    private Sprite pressedSprite;

    [Header("Light Source")]
    [SerializeField]
    private GameObject lightSource;
    private UnityEngine.Rendering.Universal.Light2D lt;
    private float maxLight;

    [Header("Button Settings")]
    [SerializeField]
    private bool forPlant;

    [Header("Reset Zone")]
    [SerializeField]
    private ResetZone resetZone; // Drag your ResetZone GameObject here

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (lightSource != null)
            lt = lightSource.GetComponent<UnityEngine.Rendering.Universal.Light2D>();

        if (lt != null)
            maxLight = lt.intensity;

        if (sr != null)
            unpressedSprite = sr.sprite;
    }

    void Update()
    {
        if (canPress && Input.GetKeyDown(KeyCode.E))
            Press();
    }

    void Press()
    {
        if (sr == null) return;

        pressed = !pressed;

        if (pressed)
        {
            sr.sprite = pressedSprite;
            if (lt != null) lt.intensity = 0;
            progress = 1;

            Debug.Log("ButtonLogic: Button pressed.");

            // Trigger ResetZone if assigned
            if (resetZone != null)
            {
                resetZone.ResetGameSection();
                Debug.Log("ButtonLogic: ResetZone triggered.");
            }
        }
        else
        {
            sr.sprite = unpressedSprite;
            if (lt != null) lt.intensity = maxLight;
            progress = 0;

            Debug.Log("ButtonLogic: Button unpressed.");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        bool isValid =
            ((forPlant && other.CompareTag("Plant")) || (!forPlant && other.CompareTag("Robot")));

        if (isValid)
            canPress = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (isPlayer(other.gameObject))
            canPress = false;
    }

    public float getProgress()
    {
        return progress;
    }

    private bool isPlayer(GameObject other)
    {
        return other.GetComponent<MoveCharacter>() != null;
    }
}
