using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEndScript : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        //Debug.Log("Particle stopped....");
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
