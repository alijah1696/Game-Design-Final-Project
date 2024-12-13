using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class GlobalLight : MonoBehaviour
{

    [SerializeField] private InteractableProxy proxy;
    private Light2D light;
    private float minIntensity;
    private float maxIntensity = 1f;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light2D>();
        minIntensity = light.intensity;
    }

    // Update is called once per frame
    void Update()
    {     
        if(proxy != null){
            if(proxy.getProgress() == 1){
                light.intensity = maxIntensity;
            }else{
                light.intensity = minIntensity;
            }
        }
    }
}
