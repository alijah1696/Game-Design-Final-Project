using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineGrappleLogic : MonoBehaviour
{   
    private Material material;
    private bool canGrapple;
    public float outlineThickness = 2f;
    public Color offColor;

    // Start is called before the first frame update
    private Color defaultMainColor; // Store the original color

    void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
        defaultMainColor = material.GetColor("_MainColor"); // Get the default color
    }

    void Update()
    {
        if (canGrapple)
        {
            material.SetFloat("_Thickness", outlineThickness);
            material.SetColor("_MainColor", defaultMainColor);
        }
        else
        {
            material.SetFloat("_Thickness", 0f);
            material.SetColor("_MainColor", offColor);
        }
    }


    void OnTriggerStay2D(Collider2D other)
    {   
        canGrapple = false;
        if (other.CompareTag("Plant") && HasVine(other.gameObject))
        {
            canGrapple = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {   
        canGrapple = false;
    }

    bool HasVine(GameObject player)
    {
        return player.GetComponent<PlantVineMovement>() != null;
    }

    public bool CanGrapple(){
        return canGrapple;
    }
}
