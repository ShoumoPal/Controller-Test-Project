using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerVFXHelper : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] VisualEffect shieldEffect;

    private PlayerInputManager inputManager;
    private BoxCollider shieldCollider;
    private bool preShield;
    private bool shieldStatus;

    private void Awake()
    {
        inputManager = GetComponentInParent<PlayerInputManager>();
        shieldCollider = GetComponentInChildren<BoxCollider>();
        shieldEffect.Stop();
    }

    private void Update()
    {
        #region Shield Control
        if (inputManager.shield && !preShield && !shieldStatus)
        {
            shieldEffect.Play();
            shieldCollider.enabled = true;
            shieldStatus = true;
            StartCoroutine(CheckShieldCR());
        }

        preShield = inputManager.shield; 
        #endregion
    }

    private IEnumerator CheckShieldCR()
    {
        yield return new WaitUntil(() => shieldEffect.HasAnySystemAwake());
        yield return new WaitUntil(() => !shieldEffect.HasAnySystemAwake());

        shieldStatus = false;
        shieldCollider.enabled = false;
    }
}
