using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private bool constant;
    [SerializeField] private float lerpStep;
    [SerializeField] private float maxIntensity;
    [SerializeField] public bool turnedOn;
    
    [Header("Refs")] private Light light;

    [Header("Other")] 
    private float lerp = 0;
    private float intensity;
    
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!turnedOn) light.intensity = 0;
        else if (!constant)
        {
            Blinking();
            light.intensity = intensity;
        }

    }

    private void Blinking()
    {
        lerp += lerpStep;
        intensity = Mathf.Lerp(0, maxIntensity, lerp);
        if (lerp >= maxIntensity && lerpStep > 0) lerpStep *= -1;
        else if (lerp < 0 && lerpStep < 0) lerpStep *= -1;
    }
}
