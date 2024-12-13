using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineGrappleLogic : MonoBehaviour
{   
    private Material material;
    private bool inRange;
    public float outlineThickness = 2f;
    public Color offColor;

    private bool onCooldown; 
    [SerializeField] private float cooldownTime;
    [SerializeField] private LayerMask obstacleLayer;  // LayerMask for detecting obstacles

    [SerializeField] private ParticleSystem ps;

    private LineRenderer lr;
    private GameObject player;

    // Start is called before the first frame update
    private Color defaultMainColor; // Store the original color

    void Start()
    {     
        if(ps != null) ps.Stop(true);

        material = GetComponent<SpriteRenderer>().material;
        defaultMainColor = material.GetColor("_MainColor"); // Get the default color

        lr = GetComponent<LineRenderer>();  
    }

    void Update()
    {   
        if (inRange && !OnCooldown())
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
    
    public void Cooldown(){
        StartCoroutine(CooldownCoroutine());
    }
    
    IEnumerator CooldownCoroutine(){
        onCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        onCooldown = false;
    }

    public bool OnCooldown(){
        return onCooldown || !CanGrapple();
    }

    public void Particle(){
        if(ps != null) StartCoroutine(ParticleCoroutine());
    }

    IEnumerator ParticleCoroutine(){
        ps.Play();
        yield return new WaitForSeconds(0.1f);
        ps.Stop(false);
    }

    bool CanGrapple()
    {
        if (player == null)
        {
            return false;
        }

        // Perform a raycast to check for obstacles
        Vector2 direction = (transform.position - player.transform.position).normalized;
        float distance = Vector2.Distance(transform.position, player.transform.position);

        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, direction, distance, obstacleLayer);

        // Return true if the raycast does not hit any obstacles
        return hit.collider == null;
    }
    
    void OnTriggerStay2D(Collider2D other)
    {   
        inRange = false;
        if (other.CompareTag("Plant") && HasVine(other.gameObject))
        {
            inRange = true;
            player = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {   
        if(other.CompareTag("Plant") || other.CompareTag("Robot")){
            inRange = false;
            player = null;
        }
    }

    //HELPER METHODS
    bool HasVine(GameObject player)
    {   
        if(player == null) return false;
        return player.GetComponent<PlantVineMovement>() != null;
    }
}
