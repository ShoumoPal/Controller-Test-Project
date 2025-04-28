using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class SunMomentScript : MonoBehaviour
{
    private HDAdditionalLightData sun;
    private float currentIntensity;

    private void Start()
    {
        sun = GetComponent<HDAdditionalLightData>();
        currentIntensity = sun.intensity;
    }

    public void StartBackToNormalFX()
    {
        StartCoroutine (BackToNormalFX());
    }

    private IEnumerator BackToNormalFX()
    {
        while (currentIntensity < 50)
        {
            currentIntensity += Time.deltaTime * 10;
            sun.SetIntensity(currentIntensity);
            yield return null;
        }
    }

    public void StartSunFX()
    {
        StartCoroutine (SunFX());
    }
    private IEnumerator SunFX()
    {
        while(currentIntensity > 0)
        {
            currentIntensity -= Time.deltaTime * 10;
            sun.SetIntensity(currentIntensity);
            yield return null;
        }
    }
}
