using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveObject : MonoBehaviour
{
    [SerializeField] float dissolveSpeed;
    private Material material;
    private float dissolveAmount;

    private void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    public void Dissolve()
    {
        StartCoroutine(DissolveCR());
    }

    private IEnumerator DissolveCR()
    {
        while (material.GetFloat("_DissolveAmount") < 1f)
        {
            dissolveAmount += dissolveSpeed * Time.deltaTime;
            dissolveAmount = Mathf.Clamp01(dissolveAmount);
            material.SetFloat("_DissolveAmount", dissolveAmount);

            yield return null;
        }

        if (material.GetFloat("_DissolveAmount") == 1f)
            Destroy(gameObject);
    }
}
