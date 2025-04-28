using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ObjectPoolManager : MonoBehaviour
{
    private static List<PooledObjectInfo> ObjectPoolList = new();
    private static GameObject pooledParent;
    private static GameObject pooledBullets;
    private static GameObject pooledEnemyBullets;
    private static GameObject pooledBulletExplosions;
    private static GameObject pooledPowerUpPickUpParticles;

    public enum PoolType
    {
        Bullet, 
        EnemyBullet,
        Bullet_Explosion,
        PowerUp_Pickup_Particles,
        None
    }
    public static PoolType PoolingType;

    private void Awake()
    {
        CreatePoolParents(); // Create Object Pool hierarchy
    }

    private void CreatePoolParents()
    {
        pooledParent = new("Pooled Objects");
        pooledBullets = new("Bullets");
        pooledEnemyBullets = new("Enemy Bullets");
        pooledBulletExplosions = new("Bullet Explosions");
        pooledPowerUpPickUpParticles = new("Powerup Pickup Particles");
        pooledBullets.transform.SetParent(pooledParent.transform);
        pooledEnemyBullets.transform.SetParent(pooledParent.transform);
        pooledPowerUpPickUpParticles.transform.SetParent(pooledParent.transform);
        pooledBulletExplosions.transform.SetParent(pooledParent.transform);
    }

    private static GameObject GetPooledParent(PoolType poolType)
    {
        var pooledParent = poolType switch
        {
            PoolType.Bullet => pooledBullets,
            PoolType.EnemyBullet => pooledEnemyBullets,
            PoolType.Bullet_Explosion => pooledBulletExplosions,
            PoolType.PowerUp_Pickup_Particles => pooledPowerUpPickUpParticles,
            PoolType.None => null,
            _ => null
        };

        return pooledParent;
    }
    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, PoolType poolType = PoolType.None)
    {
        PooledObjectInfo pool = ObjectPoolList.Find(x => x.LookUpString == objectToSpawn.name);

        if (pool == null)
        {
            pool = new() { LookUpString = objectToSpawn.name };
            ObjectPoolList.Add(pool);
        }

        GameObject spawnableObject = pool.InactiveObjects.FirstOrDefault();

        if (spawnableObject != null)
        {
            spawnableObject.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
            pool.InactiveObjects.Remove(spawnableObject);

            spawnableObject.SetActive(true);
        }
        else
        {
            spawnableObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation);
            var parentObj = GetPooledParent(poolType);
            spawnableObject.transform.SetParent(parentObj.transform, false);
            spawnableObject.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
        }
        
        return spawnableObject;
    }
    public static void ReturnObjectToPool(GameObject obj)
    {
        string obName = obj.name.Substring(0, obj.name.Length - 7);

        PooledObjectInfo pool = ObjectPoolList.Find(x => x.LookUpString == obName);
        
        if (pool == null)
        {
            Debug.LogWarning("Trying to release unpooled object!");
        }
        else
        {
            obj.SetActive(false);
            pool.InactiveObjects.Add(obj);
        }
    }
}
public class PooledObjectInfo
{
    public string LookUpString;
    public List<GameObject> InactiveObjects = new();
}
