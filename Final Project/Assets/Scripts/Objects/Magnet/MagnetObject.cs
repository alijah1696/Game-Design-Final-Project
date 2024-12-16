using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetObject : MonoBehaviour
{

    [SerializeField] private Color outlineColor;
    private SpriteRenderer sprite;
    private MagneticAbilities mag;

    private Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        mag = FindObjectOfType<MagneticAbilities>();

        sprite = GetComponent<SpriteRenderer>();

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        mag = FindObjectOfType<MagneticAbilities>();
        if(mag != null && mag.GetControlled() == this.gameObject){
            CanControl();
        }else{
            CantControl();
        }
    }

    void OnCollisionStay2D(Collision2D collision){
        if(IsPlayer(collision.gameObject)){
            rb.bodyType = RigidbodyType2D.Static;
        }
    }

    void OnCollisionEnter2D(Collision2D collision){
        if(IsPlayer(collision.gameObject)){
            rb.bodyType = RigidbodyType2D.Static;
        }
    }

    void OnCollisionExit2D(Collision2D collision){
        if(IsPlayer(collision.gameObject)){
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    bool IsPlayer(GameObject g){
        return (g.CompareTag("Plant") || g.CompareTag("Robot"));
    }

    void CanControl(){
        sprite.material.SetColor("_Color", outlineColor);
        sprite.material.SetFloat("_Thickness", 3f);
    }

    void CantControl(){
        sprite.material.SetColor("_Color", Color.black);
        sprite.material.SetFloat("_Thickness", 0f);
    }
}
