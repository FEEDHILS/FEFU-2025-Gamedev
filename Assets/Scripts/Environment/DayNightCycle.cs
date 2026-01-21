using System;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public float Cycle = 90;
    [SerializeField] float Speed = 0.1f;

    [SerializeField] Material DaySky, NightSky;
    [SerializeField] Light DirectLight;
    [SerializeField] Gradient DayNightColors;

    float lastCycle;


    public static DayNightCycle instance;
    void OnEnable() => instance = this;
    void OnDisable() => instance = null;

    void Start()
    {
        Material tempSky = Instantiate(RenderSettings.skybox);
        RenderSettings.skybox = tempSky;
        lastCycle = Cycle;
    }

    
    void Update()
    {
        Cycle = (Cycle + Speed*Time.deltaTime) % 360;
        
        // Чтобы не обновлять каждый кадр, обновляем при "значительных" изменениях
        if (Mathf.Abs(Cycle - lastCycle) > 0.5)
        {
            lastCycle = Cycle;
            SkyUpdate();
        }
    }

    [ContextMenu("Evaluate")]
    void SkyUpdate()
    {
        // float coeff = Mathf.Max(0, Mathf.Sin(Cycle * Mathf.Deg2Rad));

        Color currentColor = DayNightColors.Evaluate(Cycle / 360);
        RenderSettings.fogColor = currentColor;
        
        RenderSettings.ambientLight = currentColor;
        DirectLight.transform.rotation = Quaternion.Euler(Cycle-90, 0, 0);
        DirectLight.color = currentColor;
        
        RenderSettings.skybox.SetColor("_Tint", currentColor);
    }
}
