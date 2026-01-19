using System;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] float Cycle = 90;
    [SerializeField] float Speed = 0.1f;

    [SerializeField] Material DaySky, NightSky;
    [SerializeField] Color DayColor, NightColor;
    [SerializeField] Light DirectLight;
    
    private static readonly int TintID = Shader.PropertyToID("_Tint");
    private static readonly int ExposureID = Shader.PropertyToID("_Exposure");

    Material currentSky;
    private Color dayTint;
    private Color nightTint;
    private float dayExposure;
    private float nightExposure;
    private Color dayFog;
    private Color nightFog;
    float lastCycle;
    void Start()
    {
        currentSky = Instantiate(RenderSettings.skybox);
        RenderSettings.skybox = currentSky;

        dayTint = DaySky.GetColor(TintID);
        nightTint = NightSky.GetColor(TintID);
        dayExposure = DaySky.GetFloat(ExposureID);
        nightExposure = NightSky.GetFloat(ExposureID);
        dayFog = DaySky.GetColor("_FogColor");
        nightFog = NightSky.GetColor("_FogColor");
        lastCycle = Cycle;
    }

    
    void Update()
    {
        // Cycle += Speed * Time.deltaTime;
        
        // if ((Cycle - lastCycle) > 1)
        // {
        //     lastCycle = Cycle;
        //     SkyUpdate();
        // }
    }

    void SkyUpdate()
    {
        float coeff = (Mathf.Sin(Cycle * Mathf.Deg2Rad) + 1f) * 0.5f;
        print(coeff);
        
        currentSky.SetColor("_Tint", Color.Lerp(nightTint, dayTint, coeff));
        currentSky.SetFloat("_Exposure", Mathf.Lerp(nightExposure, dayExposure, coeff));
        
        Color FogColor = Color.Lerp(nightFog, dayFog, coeff);
        currentSky.SetColor("_FogColor", FogColor);
        RenderSettings.fogColor = FogColor;
        
        RenderSettings.ambientSkyColor = Color.Lerp(NightColor, DayColor, coeff);
        DirectLight.transform.rotation = Quaternion.Euler(Cycle, 0, 0);
    }
}
