using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetSpecialEffects : MonoBehaviour
{   
    [SerializeField] private GameObject circleParticlesSource;
    private ParticleSystem circleParticles;
    private float baseCircleParticleSize;

    private MagneticAbilities mag;

    // Start is called before the first frame update
    void Start()
    {
        circleParticles = circleParticlesSource.GetComponent<ParticleSystem>();
        baseCircleParticleSize = circleParticles.main.startSize.constant/1.5f;
        mag = FindObjectOfType<MagneticAbilities>();
    }

    // Update is called once per frame
    void Update()
    {
        if(mag != null){
            GameObject c = mag.GetControlled();
            circleParticlesSource.SetActive(mag.IsControlling());
            if(mag.IsControlling() && c != null){
                circleParticles.transform.position = c.transform.position;
                AdjustParticleSize(ObjectScaleFactor(c));
                if(!circleParticles.isPlaying) circleParticles.Play();
            }else{
                circleParticles.Stop();
            }
        }
    }

    void AdjustParticleSize(float sizeMultiplier)
    {
        var mainModule = circleParticles.main;
        mainModule.startSize = baseCircleParticleSize * sizeMultiplier;
    }
    
    float ObjectScaleFactor(GameObject g)
    {
        if(g == null) return 0;

        Vector3 scale = g.transform.localScale;
        return (scale.x + scale.y)/2f;
    }
}
