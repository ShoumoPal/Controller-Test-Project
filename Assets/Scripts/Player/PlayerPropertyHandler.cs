using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPropertyHandler : MonoBehaviour, IDamagable
{
    [field : SerializeField]
    public float Health { get; set; } = 100f;
    public float CurrentHealth { get; set; }

    [SerializeField] SplinterExplosion playerSplinters;
    [SerializeField] CinemachineFreeLook cam;
    [SerializeField] float shakeDuration;

    private CinemachineBasicMultiChannelPerlin middleRigNoise;
    private float shakeTimer;

    private void Start()
    {
        middleRigNoise = cam.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        CurrentHealth = Health;
    }
    private void Update()
    {
        if(shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if(shakeTimer <= 0)
            {
                middleRigNoise.m_AmplitudeGain = 0f;
                middleRigNoise.m_FrequencyGain = 0f;
            }
        }
    }
    public void OnKilled()
    {
        cam.Follow = null;
        middleRigNoise.m_AmplitudeGain = 0f;
        middleRigNoise.m_FrequencyGain = 0f;
        PlayerHUDScript.Instance.SetGameOver();
        Instantiate(playerSplinters, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        ShakeCamera();
        PlayerHUDScript.Instance.UpdateHealthSlider(damage);
        CurrentHealth -= damage;
        if (CurrentHealth <= 0f)
            OnKilled();
    }

    private void ShakeCamera()
    {
        middleRigNoise.m_AmplitudeGain = 1f;
        middleRigNoise.m_FrequencyGain = 1f;
        shakeTimer = shakeDuration;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.collider.GetComponent<LevelSphereTrigger>() != null)
        {
            var sphere = hit.collider.GetComponent<LevelSphereTrigger>();
            if (!sphere.startedLevel)
                sphere.StartLevel();
        }
    }
}
