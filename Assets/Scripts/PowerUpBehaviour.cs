using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBehaviour : MonoBehaviour
{
    [SerializeField] GameObject pickupEffect;

    private PlayerMovement player;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

    private void Update()
    {
        if(player != null && Vector3.Distance(transform.position, new(player.transform.position.x, transform.position.y, player.transform.position.z)) < 1f)
        {
            ObjectPoolManager.SpawnObject(pickupEffect, transform.position, Quaternion.identity, ObjectPoolManager.PoolType.PowerUp_Pickup_Particles);
            Destroy(gameObject);
        }
    }
}
