using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class WavePoint : MonoBehaviour
{
    public float velocity = 0;
    public float force = 0;
    // current height
    public float height = 0f;
    // normal height
    private float target_height = 0f;
    public Transform springTransform;
    
    [SerializeField]
    private SpriteShapeController spriteShapeController = null;
    private int waveIndex = 0;
    private List<WavePoint> springs = new();

    private ParticleSystem particles;
    public Color particle_Color;
    private float particle_originalSize;

    public bool shouldPlay = true;

    private float resistance = 40f;

    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
        particle_originalSize = particles.main.startSize.constant;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(SpriteShapeController ssc, float r) { 

        var index = transform.GetSiblingIndex();
        waveIndex = index+1;
        
        spriteShapeController = ssc;
        velocity = 0;
        height = transform.localPosition.y;
        target_height = transform.localPosition.y;

        resistance = r;
    }

    public void WaveSpringUpdate(float springStiffness, float dampening) { 
        height = transform.localPosition.y;
        // maximum extension
        var x = height - target_height;
        var loss = -dampening * velocity;

        force = - springStiffness * x + loss;
        velocity += force;
        var y = transform.localPosition.y;  
        transform.localPosition = new Vector3(transform.localPosition.x, y+velocity, transform.localPosition.z);
    }

    public void WavePointUpdate() { 
        if (spriteShapeController != null) {
            Spline waveSpline = spriteShapeController.spline;
            Vector3 wavePosition = waveSpline.GetPosition(waveIndex);
            waveSpline.SetPosition(waveIndex, new Vector3(wavePosition.x, transform.localPosition.y, wavePosition.z));
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (IsPlayer(other.gameObject)){
            Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
            MoveCharacter mv = other.gameObject.GetComponent<MoveCharacter>();

            var speed = rb.velocity;

            velocity += (speed.y/resistance) * ((speed.y < 0) ? 1.1f : 1f);
            velocity +=  mv.moveSpeed/resistance;

            if(Mathf.Abs(speed.y) >= 1f && Random.Range(0f, 1f) > 0.5f && shouldPlay) PlayParticle(mv.moveSpeed/8f);

        }else if(isBlock(other.gameObject, other)){
            Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();

            var speed = rb.velocity;
            Vector3 size = other.gameObject.transform.localScale;
            
            float sizeMulti = (size.x + size.y)/2f;
            speed *= sizeMulti;
    
            velocity += speed.y/resistance;
            velocity += Mathf.Abs(speed.x)/resistance;

            if(speed.y != 0 && Random.Range(0f, 1f) > 0.75f && shouldPlay) PlayParticle(sizeMulti);
        }

    }

    private bool IsPlayer(GameObject other){
        return other.gameObject.GetComponent<MoveCharacter>() != null;
    }

    private bool isBlock(GameObject other, Collider2D collider){
        return (
            (other.gameObject.GetComponent<MagnetObject>() != null) && 
            (!collider.isTrigger)
                );
    }

    public void PlayParticle(float sizeMulti){
        var main = particles.main;
        main.startColor = particle_Color;
        main.startSize = particle_originalSize * sizeMulti;

        particles.Play();
    }
}
