using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    PlayerInputManager inputManager;
    PlayerGunHandler gunHandler;

    float bulletInterval;
    float elapsedTime;
    bool firstBullet;
    bool tapFireCooldown;
    bool preReload;
    bool isReloading;
    bool isShooting;

    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private Bullet bullet;
    [SerializeField] private float fireRate;
    [SerializeField] private float magSize;
    [SerializeField] private float currentMagSize;
    [SerializeField] private float magReserves;
    [SerializeField] private float reloadTime;
    [SerializeField] private GunType type;

    public bool isPickable;
    public bool isThrowable;

    float passedReloadTime;


    private void Start()
    {
        inputManager = FindObjectOfType<PlayerInputManager>();
        gunHandler = inputManager.GetComponent<PlayerGunHandler>();
        bulletInterval = (60 / fireRate);
        currentMagSize = magSize;
    }

    private void Update()
    {
        bool isCurrentGun = gunHandler.GetCurrentGun() == this;

        if(!isCurrentGun)
        {
            isReloading = false;
            isShooting = false;
        }

        if (inputManager && gunHandler.GetCurrentGun() != null && isCurrentGun)
        {
            Shoot();

            ReloadGun();
        }

        if (isReloading && isCurrentGun)
        {
            PlayerHUDScript.Instance.UpdateReloadUI(passedReloadTime / reloadTime);
            passedReloadTime += Time.deltaTime;
        }
        else if(isCurrentGun)
        {
            PlayerHUDScript.Instance.ResetReloadUI();
            passedReloadTime = 0f;
        }
    }

    private void Shoot()
    {
        if (inputManager.shoot && currentMagSize != 0 && !isReloading)
        {
            isShooting = true;

            if (!firstBullet)
            {
                firstBullet = true;
                SpawnBullet();
                //StartCoroutine(TapFireCooldownCR());
            }

            if (elapsedTime >= bulletInterval)
            {
                SpawnBullet();
                elapsedTime = 0f;
            }

            elapsedTime += Time.deltaTime;
        }
        else
        {
            if(currentMagSize == 0 && inputManager.shoot && !isReloading)
            {
                StartCoroutine(ReloadGunCR());
            }
            isShooting = false;
            firstBullet = false;
            elapsedTime = 0f;
        }           
    }

    private void ReloadGun()
    {
        if(inputManager.reload && currentMagSize != magSize && !isShooting && gunHandler.GetCurrentGun() != null && gunHandler.GetCurrentGun() == this)
        {
            if(inputManager.reload && !preReload && !isReloading)
            {
                StartCoroutine(ReloadGunCR());
            }
        }
        preReload = inputManager.reload;
    }

    private IEnumerator TapFireCooldownCR()
    {
        SpawnBullet();
        yield return new WaitForSeconds(bulletInterval);
        yield return new WaitUntil(() => !inputManager.shoot);
        firstBullet = false;
    }

    private void SpawnBullet()
    {
        var spawnBullet = ObjectPoolManager.SpawnObject(bullet.gameObject, bulletSpawnPoint.position, bulletSpawnPoint.rotation, ObjectPoolManager.PoolType.Bullet);
        spawnBullet.GetComponent<Bullet>().SetVelocity(bulletSpawnPoint.forward);

        currentMagSize -= 1;
        PlayerHUDScript.Instance.UpdateGunInfo(currentMagSize, magReserves);
        //spawnBullet.transform.localEulerAngles = new Vector3(0f, inputManager.transform.localEulerAngles.y, 0f);
    }

    private IEnumerator ReloadGunCR()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);

        var bulletsReloaded = magSize - currentMagSize;
        if(magReserves - bulletsReloaded < 0f)
        {
            bulletsReloaded = magReserves;
        }
        currentMagSize = magSize;
        magReserves -= bulletsReloaded;
        isReloading = false;

        PlayerHUDScript.Instance.UpdateGunInfo(currentMagSize, magReserves);
    }

    public float GetMagSize()
    {
        return magSize;
    }
    public float GetMagReserves()
    {
        return magReserves;
    }
    public float GetCurrentMagSize()
    {
        return currentMagSize;
    }
}
public enum GunType
{
    Pistol,
    Assault_Rifle
}
