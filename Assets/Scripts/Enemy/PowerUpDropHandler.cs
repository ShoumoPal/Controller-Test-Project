using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpDropHandler : MonoBehaviour
{
    [SerializeField] private GameObject powerUpPrefab;
    [SerializeField] private float oddsPercent;

    public void GeneratePowerUp()
    {
        var percent = Random.Range(1, 100);
        if(oddsPercent <= percent)
        {
            Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
        }
    }
}
