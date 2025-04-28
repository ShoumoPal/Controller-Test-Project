using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarCollisionCheck : MonoBehaviour
{
    [SerializeField] Material breakableMaterial;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    private void OnParticleCollision(GameObject other)
    {
        meshRenderer.material = breakableMaterial;
        gameObject.layer = LayerMask.NameToLayer("Ground");
    }
}
