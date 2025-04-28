using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHUDScript : MonoBehaviour
{
    public static PlayerHUDScript Instance;

    [SerializeField] SpriteRenderer _pickupPrompt;
    [SerializeField] GameObject _gunInfo;
    [SerializeField] Image reticle;
    [SerializeField] Image reloadUI;
    [SerializeField] Slider playerHealth;
    [SerializeField] Slider easeHealth;
    [SerializeField] Image playerHealthFillImage;
    [SerializeField] float healthDamageSpeed;
    [SerializeField] CanvasGroup loadingScreen;
    [SerializeField] CanvasGroup gameOverScreen;

    private Color currentHealthBarColor;
    private PlayerPropertyHandler player;

    private void Awake()
    {
        #region Singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject); 
        #endregion

        currentHealthBarColor = playerHealthFillImage.color;
        player = FindObjectOfType<PlayerPropertyHandler>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && !Cursor.visible)
        {
            Cursor.visible = true;
        }
        if (Input.GetKeyDown(KeyCode.N) && Cursor.visible)
        {
            Cursor.visible = false;
        }
    }

    public SpriteRenderer GetPickupPromptUI()
    {
        return _pickupPrompt;
    }

    #region Gun Info
    public void UpdateGunInfo(float magSize, float magReserves) // Overloaded function
    {
        var textMesh = _gunInfo.GetComponentInChildren<TextMeshProUGUI>();
        textMesh.text = $"{magSize}/{magReserves}";

        if (!_gunInfo.activeSelf)
            _gunInfo.SetActive(true);
    }
    public void UpdateGunInfo(bool state)
    {
        _gunInfo.SetActive(state);
    } 
    #endregion

    public void SetReticleVisibility(bool state)
    {
        reticle.gameObject.SetActive(state);
    }
    public void UpdateReloadUI(float reloadTime)
    {
        reloadUI.fillAmount = Mathf.Lerp(0f, 1f, reloadTime);
    }
    public void ResetReloadUI()
    {
        reloadUI.fillAmount = 0f;
    }

    public void LoadScene(int index)
    {
        StartCoroutine(LoadSceneCR(index));
    }

    private IEnumerator LoadSceneCR(int index)
    {
        while (loadingScreen.alpha != 1)
        {
            loadingScreen.alpha = Mathf.Lerp(0f, 1f, loadingScreen.alpha + Time.deltaTime * 2);
            yield return null;
        }

        SceneManager.LoadSceneAsync(index);
    }
    #region Player Health
    public void UpdateHealthSlider(float damage)
    {
        StartCoroutine(UpdateHealthSliderCR(damage));
    }

    private IEnumerator UpdateHealthSliderCR(float damage)
    {
        playerHealth.value = Mathf.InverseLerp(0f, player.Health, player.CurrentHealth - damage);

        while(playerHealth.value < easeHealth.value)
        {
            easeHealth.value = Mathf.Lerp(easeHealth.value, playerHealth.value, Time.deltaTime * healthDamageSpeed);
            yield return null;
        }
    }
    #endregion

    #region Game Over
    public void SetGameOver()
    {
        StartCoroutine(GameOverCR());
    }

    private IEnumerator GameOverCR()
    {
        while(gameOverScreen.alpha != 1f)
        {
            gameOverScreen.alpha = Mathf.Lerp(0f, 1f, gameOverScreen.alpha + Time.deltaTime * 3);
            yield return null;
        }

        gameOverScreen.blocksRaycasts = true;
        gameOverScreen.interactable = true;
    }
    #endregion
}
