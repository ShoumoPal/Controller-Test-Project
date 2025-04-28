using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;

public class PlayerGunHandler : MonoBehaviour
{
    [SerializeField] private GunScript _currentGun;
    [SerializeField] private GunScript _currentPickableGun;
    [SerializeField] private Transform _gunHolster;
    [SerializeField] private float throwForce;

    PlayerInputManager inputManager;

    private void Start()
    {
        inputManager = GetComponent<PlayerInputManager>();
        if(_currentGun != null)
        {
            PlayerHUDScript.Instance.UpdateGunInfo(_currentGun.GetCurrentMagSize(), _currentGun.GetMagReserves());
        }
    }

    public GunScript GetCurrentGun()
    {
        return _currentGun;
    }
    public void SetCurrentGun(GunScript gun)
    {
        _currentPickableGun = gun;
    }
    public void ResetCurrentGun()
    {
        _currentPickableGun = null;
    }

    private void Update()
    {
        if( _currentPickableGun != null)
        {
            if (_currentPickableGun.isPickable && inputManager.pickup) // Pickup Gun
            {
                PickUpGun();
                PlayerHUDScript.Instance.GetPickupPromptUI().enabled = false;
            }
        } 

        if(_currentGun != null)
        {
            if(_currentGun.isThrowable && inputManager.drop)
            {
                DropGun();
            }
        }
    }

    private void DropGun()
    {
        _currentGun.gameObject.RemoveParentConstraint();
        _currentGun.gameObject.AddComponent<Rigidbody>().mass = 5f;
        EnableColliders(_currentGun);

        var rb = _currentGun.GetComponent<Rigidbody>();
        rb.AddForce((_gunHolster.transform.forward + Vector3.up) * throwForce, ForceMode.Impulse);
        rb.AddTorque(new(UnityEngine.Random.Range(-1f, 1f),
                         UnityEngine.Random.Range(-1f, 1f),
                         UnityEngine.Random.Range(-1f, 1f)), ForceMode.Impulse);

        _currentGun.isPickable = true;
        _currentGun.isThrowable = false;

        _currentGun = null;

        PlayerHUDScript.Instance.UpdateGunInfo(false);
        PlayerHUDScript.Instance.ResetReloadUI();
    }

    private void PickUpGun()
    {
        if(_currentGun != null)
        {
            DropGun();
        }

        _currentPickableGun.transform.position = _gunHolster.position;
        _currentPickableGun.transform.rotation = _gunHolster.rotation;
        _currentPickableGun.gameObject.AddParentConstraint(_gunHolster);

        _currentPickableGun.isPickable = false;
        _currentPickableGun.isThrowable = true;

        _currentGun = _currentPickableGun;
        _currentPickableGun = null;
        Destroy(_currentGun.GetComponent<Rigidbody>());
        DisableColliders(_currentGun);

        PlayerHUDScript.Instance.UpdateGunInfo(_currentGun.GetCurrentMagSize(), _currentGun.GetMagReserves());
    }

    private void EnableColliders(GunScript currentGun)
    {
        List<BoxCollider> colliders = currentGun.GetComponentsInChildren<BoxCollider>().ToList();
        if(colliders.Count > 0)
            colliders.ForEach(collider => collider.enabled = true);
    }
    private void DisableColliders(GunScript currentGun)
    {
        List<BoxCollider> colliders = currentGun.GetComponentsInChildren<BoxCollider>().ToList();
        if (colliders.Count > 0)
            colliders.ForEach(collider => collider.enabled = false);
    }
}
