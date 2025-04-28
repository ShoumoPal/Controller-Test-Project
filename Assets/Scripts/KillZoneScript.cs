using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZoneScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Killed splinters....");

        if(other.transform.parent != null)
            Destroy(other.transform.parent.gameObject);
        else
            Destroy(other.gameObject);
    }
}
