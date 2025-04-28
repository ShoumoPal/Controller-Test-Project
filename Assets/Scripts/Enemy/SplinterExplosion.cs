using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplinterExplosion : MonoBehaviour
{
    public float minForce;
    public float maxForce;
    public float radius;
    public float yOffset;
    public float removeColliderTimer;

    private void Start()
    {
        Explode();
        StartCoroutine(RemoveAllColliders());
    }

    private void Explode()
    {
        foreach(Transform child in transform)
        {
            Rigidbody rb = child.GetComponent<Rigidbody>();
            Vector3 explosionPos = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);
            rb.AddExplosionForce(Random.Range(minForce, maxForce), explosionPos, radius);
        }
    }
    private IEnumerator RemoveAllColliders()
    {
        yield return new WaitForSeconds(removeColliderTimer);

        foreach (Transform child in transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer("Splinters");
        }
    }
}
