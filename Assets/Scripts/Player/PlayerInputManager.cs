using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    PlayerMovement player;
    public bool jump;
    public bool dash;
    public bool sprint;
    public bool aim;
    public bool shoot;
    public bool pickup;
    public bool drop;
    public bool reload;
    public bool scan;
    public bool shield;

    PlayerControls playerControls;

    private void OnEnable()
    {
        playerControls = new();
    }
    private void Start()
    {
        player = GetComponent<PlayerMovement>();
        playerControls.Enable();
        playerControls.Player.Movement.performed += UpdateMovement;
        playerControls.Player.Movement.canceled += CancelMovement;
        playerControls.Player.Sprint.performed += Sprint;
        playerControls.Player.Sprint.canceled += CancelSprint;
        playerControls.Player.Jump.performed += Jump;
        playerControls.Player.Dash.performed += Dash;
        playerControls.Player.Dash.canceled += CancelDash;
        playerControls.Player.Aim.performed += Aim;
        playerControls.Player.Aim.canceled += CancelAim;
        playerControls.Player.Shoot.performed += Shoot;
        playerControls.Player.Shoot.canceled += CancelShoot;
        playerControls.Player.Pickup.performed += Pickup;
        playerControls.Player.Pickup.canceled += CancelPickup;
        playerControls.Player.Drop.performed += Drop;
        playerControls.Player.Drop.canceled += CancelDrop;
        playerControls.Player.Reload.performed += Reload;
        playerControls.Player.Reload.canceled += CancelReload;
        playerControls.Player.Scan.performed += Scan;
        playerControls.Player.Scan.canceled += CancelScan;
        playerControls.Player.Shield.performed += Shield;
        playerControls.Player.Shield.canceled += CancelShield;
    }

    #region Shield
    private void Shield(InputAction.CallbackContext context)
    {
        shield = true;
    }
    private void CancelShield(InputAction.CallbackContext context)
    {
        shield = false;
    } 
    #endregion

    #region Scan
    private void Scan(InputAction.CallbackContext context)
    {
        scan = true;
    }
    private void CancelScan(InputAction.CallbackContext context)
    {
        scan = false;
    } 
    #endregion

    #region Reload
    private void Reload(InputAction.CallbackContext context)
    {
        reload = true;
    }
    private void CancelReload(InputAction.CallbackContext context)
    {
        reload = false;
    } 
    #endregion

    #region Drop
    private void Drop(InputAction.CallbackContext context)
    {
        drop = true;
    }
    private void CancelDrop(InputAction.CallbackContext context)
    {
        drop = false;
    } 
    #endregion

    #region Pickup
    private void Pickup(InputAction.CallbackContext context)
    {
        pickup = true;
    }

    private void CancelPickup(InputAction.CallbackContext context)
    {
        pickup = false;
    } 
    #endregion

    #region Shoot
    private void Shoot(InputAction.CallbackContext context)
    {
        shoot = true;
    }
    private void CancelShoot(InputAction.CallbackContext context)
    {
        shoot = false;
    } 
    #endregion

    #region Aim
    private void Aim(InputAction.CallbackContext context)
    {
        aim = true;
        player.SetRecenter(false);
        PlayerHUDScript.Instance.SetReticleVisibility(true);
    }
    private void CancelAim(InputAction.CallbackContext context)
    {
        aim = false;
        player.SetRecenter(true);
        PlayerHUDScript.Instance.SetReticleVisibility(false);
    }
    #endregion

    #region Dash
    private void Dash(InputAction.CallbackContext context)
    {
        dash = true;
    }
    private void CancelDash(InputAction.CallbackContext context)
    {
        dash= false;
    }
    #endregion

    #region Jump
    private void Jump(InputAction.CallbackContext context)
    {
        jump = true;
    }
    #endregion

    #region Sprint
    private void Sprint(InputAction.CallbackContext context)
    {
        if (player.IsGrounded)
        {
            sprint = true;
            player.Sprint();
        }
    }
    private void CancelSprint(InputAction.CallbackContext context)
    {
        if (sprint)
        {
            sprint = false;
            player.CancelSprint();
        }
    }
    #endregion

    #region Movement Related
    private void CancelMovement(InputAction.CallbackContext context)
    {
        player.direction = new Vector3(context.ReadValue<Vector2>().x, 0f, context.ReadValue<Vector2>().y).normalized;
    }

    private void UpdateMovement(InputAction.CallbackContext context)
    {
        //Debug.Log(context);
        player.direction = new Vector3(context.ReadValue<Vector2>().x, 0f, context.ReadValue<Vector2>().y).normalized;
    }
    #endregion

    public void EnablePlayerControls()
    {
        playerControls.Enable();
    }
    public void DisablePlayerControls()
    {
        playerControls.Disable();
    }
    public PlayerControls GetPlayerControls()
    {
        return playerControls;
    }
}
