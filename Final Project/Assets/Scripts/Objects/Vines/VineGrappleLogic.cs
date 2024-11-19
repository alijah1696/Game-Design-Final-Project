using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineGrappleLogic : MonoBehaviour
{   
    private Material material;
    private bool canGrapple;
    public float outlineThickness = 2f;
    public Color offColor;

    private LineRenderer lr;
    private GameObject player;

    // Start is called before the first frame update
    private Color defaultMainColor; // Store the original color

    void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
        defaultMainColor = material.GetColor("_MainColor"); // Get the default color

        lr = GetComponent<LineRenderer>();  
    }

    void Update()
    {   
        if (canGrapple)
        {
            material.SetFloat("_Thickness", outlineThickness);
            material.SetColor("_MainColor", defaultMainColor);

            //logic for drawing vine to player when grappling
            bool shouldDraw = false;
            if(HasVine(player)) shouldDraw = player.GetComponent<PlantVineMovement>().IsGrappling();
            
            if(shouldDraw){
                DrawVineTo(player); 
            }else{
                CutVine();
            }
        }
        else
        {
            material.SetFloat("_Thickness", 0f);
            material.SetColor("_MainColor", offColor);

            CutVine();
        }
    }


    
    void OnTriggerStay2D(Collider2D other)
    {   
        canGrapple = false;
        if (other.CompareTag("Plant") && HasVine(other.gameObject))
        {
            canGrapple = true;
            player = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {   
        if(other.CompareTag("Plant") || other.CompareTag("Robot")){
            player = null;
            canGrapple = false;
        }
    }

    //ACTIVE METHODS
    public void DrawVineTo(GameObject player){
        lr.SetPosition(0, transform.position);
        if(player != null){
            lr.SetPosition(1, player.transform.position);
        }else{
            CutVine();
        }
    }

    public void CutVine(){
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, transform.position);
    }


    //HELPER METHODS
    bool HasVine(GameObject player)
    {   
        if(player == null) return false;
        return player.GetComponent<PlantVineMovement>() != null;
    }

    public bool CanGrapple(){
        return canGrapple;
    }

}
