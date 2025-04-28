using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsatingGlow : MonoBehaviour
{
    [SerializeField] private float pulseSpeed;
    [SerializeField] private float minIntensity;
    [SerializeField] private float maxIntensity;
    [SerializeField] private Color emissionColor;

    private Material mat;
    private Renderer targetRenderer;

    private void Start()
    {
        targetRenderer = GetComponent<Renderer>();
        mat = targetRenderer.material;
    }

    private void Update()
    {
        var emissionStrength = Mathf.Lerp(minIntensity, maxIntensity, (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);
        var finalColor = emissionColor * emissionStrength;

        mat.SetColor("_EmissiveColor", finalColor);

        //DynamicGI.SetEmissive(targetRenderer, finalColor);
    }
}
