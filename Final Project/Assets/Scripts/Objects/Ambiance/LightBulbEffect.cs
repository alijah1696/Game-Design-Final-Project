using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LightBulbEffect : MonoBehaviour
{
    [SerializeField] private LightBulbSetting setting;
    public bool isOn = true;

    UnityEngine.Rendering.Universal.Light2D lt;
    private float maxIntensity;

    // Flicker settings
    public float flickerIntervalMin = 0.1f;
    public float flickerIntervalMax = 0.5f;

    // Dimmed settings
    public float dimmedIntensityMin = 0.2f;
    public float dimmedIntensityMax = 0.8f;

    [Range(0.0F, 10.0F)]
    public float dimmingSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        lt = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        maxIntensity = lt.intensity;

        if (setting == LightBulbSetting.Flicker)
        {
            StartCoroutine(FlickerLight());
        }
        else if (setting == LightBulbSetting.Dimmed)
        {
            StartCoroutine(DimmedLight());
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(setting == LightBulbSetting.Constant) lt.intensity = maxIntensity;
        lt.intensity *= isOn ? 1 : 0;
    }

    IEnumerator FlickerLight()
    {
        while (true)
        {
            if (setting == LightBulbSetting.Flicker)
            {
                isOn = !isOn;
                yield return new WaitForSeconds(Random.Range(flickerIntervalMin, flickerIntervalMax));
            }
            else
            {
                yield break;
            }
        }
    }

    IEnumerator DimmedLight()
    {
        while (true)
        {
            if (setting == LightBulbSetting.Dimmed)
            {
                float elapsed = 0f;
                float startIntensity = lt.intensity;
                float targetIntensity = Random.Range(dimmedIntensityMin * maxIntensity, dimmedIntensityMax * maxIntensity);

                while (elapsed < dimmingSpeed)
                {
                    elapsed += Time.deltaTime;
                    float progress = elapsed / dimmingSpeed;
                    Debug.Log(progress);
                    lt.intensity = Mathf.Lerp(startIntensity, targetIntensity, progress);
                    yield return null;
                }
            }
            else
            {
                yield break;
            }
        }
    }
}

public enum LightBulbSetting
{
    Constant,
    Flicker,
    Dimmed
}