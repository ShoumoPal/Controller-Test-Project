using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainScanHandler : MonoBehaviour
{
    [SerializeField] GameObject terrainScanObj;
    [SerializeField] float duration;
    [SerializeField] float size;

    private PlayerInputManager inputManager;
    private bool preScan;

    private void Awake()
    {
        inputManager = GetComponent<PlayerInputManager>();
    }

    private void Update()
    {
        if(inputManager.scan && !preScan)
        {
            GameObject scanObj = Instantiate(terrainScanObj, transform.position, Quaternion.identity);
            ParticleSystem scanParticle = scanObj.transform.GetChild(0).GetComponent<ParticleSystem>();

            if(scanParticle != null )
            {
                var main = scanParticle.main;
                main.startSize = size;
                main.startLifetime = duration;
            }

            Destroy(scanObj, duration);
        }

        preScan = inputManager.scan;
    }
}
