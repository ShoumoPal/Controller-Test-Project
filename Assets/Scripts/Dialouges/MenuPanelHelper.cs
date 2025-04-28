using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPanelHelper : MonoBehaviour
{
    [SerializeField] Button backButton;
    [SerializeField] CanvasGroup menuGroup;
    [SerializeField] CanvasGroup controlsGroup;

    private Image panel;
    private RectTransform panelTransform;
    private bool isMenuOn;
    private Tween checkTween;
    private Coroutine checkCoroutine;

    private void Start()
    {
        panel = GetComponent<Image>();
        panelTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isMenuOn)
            ShowMenu();
        else if (Input.GetKeyDown(KeyCode.Escape) && isMenuOn)
            HideMenu();
    }

    #region Menu System
    public void HideMenu()
    {
        isMenuOn = false;
        if (checkTween.IsActive())
            checkTween.Kill();

        if (checkCoroutine != null)
            StopCoroutine(checkCoroutine);

        checkCoroutine = StartCoroutine(HideMenuCR());
    }

    private IEnumerator HideMenuCR()
    {
        checkTween = panelTransform.DOAnchorPos(new(-480f, 0f), 0.5f).SetEase(Ease.InOutFlash).SetUpdate(true);
        panel.DOFade(0f, 1f).SetEase(Ease.Linear).SetUpdate(true);

        yield return new WaitUntil(() => !checkTween.IsActive());
        backButton.gameObject.SetActive(false);

        // Time back to normal
        Time.timeScale = 1f;
        FindObjectOfType<PlayerInputManager>().EnablePlayerControls();
    }

    private void ShowMenu()
    {
        // Time stop
        Time.timeScale = 0f;
        FindObjectOfType<PlayerInputManager>().DisablePlayerControls();

        isMenuOn = true;
        if (checkTween.IsActive())
            checkTween.Kill();

        if (checkCoroutine != null)
            StopCoroutine(checkCoroutine);

        checkCoroutine = StartCoroutine(ShowMenuCR());
    }

    private IEnumerator ShowMenuCR()
    {
        checkTween = panelTransform.DOAnchorPos(new(480f, 0f), 0.5f).SetEase(Ease.InOutFlash).SetUpdate(true);
        panel.DOFade(0.8f, 1f).SetEase(Ease.Linear).SetUpdate(true);

        yield return new WaitUntil(() => !checkTween.IsActive());
        backButton.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        PlayerHUDScript.Instance.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    #endregion

    #region Controls Menu
    public void ShowControls()
    {
        StartCoroutine(ShowControlsCR());
    }

    private IEnumerator ShowControlsCR()
    {
        checkTween = menuGroup.GetComponent<RectTransform>().DOAnchorPos(new(-710f, 345.5f), 0.5f).SetEase(Ease.Linear).SetUpdate(true);
        menuGroup.DOFade(0f, 0.5f).SetEase(Ease.Linear).SetUpdate(true);
        menuGroup.interactable = false;
        menuGroup.blocksRaycasts = false;

        yield return new WaitUntil(() => !checkTween.IsActive());

        controlsGroup.GetComponent<RectTransform>().anchoredPosition = new(-230f, 345.5f);
        controlsGroup.DOFade(1f, 0.5f).SetEase(Ease.Linear).SetUpdate(true);
        controlsGroup.interactable = true;
        controlsGroup.blocksRaycasts = true;
    }

    public void HideControls()
    {
        StartCoroutine(HideControlsCR());
    }

    private IEnumerator HideControlsCR()
    {
        checkTween = controlsGroup.GetComponent<RectTransform>().DOAnchorPos(new(-710f, 345.5f), 0.5f).SetEase(Ease.Linear).SetUpdate(true);
        controlsGroup.DOFade(0f, 0.5f).SetEase(Ease.Linear).SetUpdate(true);
        controlsGroup.interactable = false;
        controlsGroup.blocksRaycasts = false;

        yield return new WaitUntil(() => !checkTween.IsActive());

        menuGroup.GetComponent<RectTransform>().anchoredPosition = new(-230f, 345.5f);
        menuGroup.DOFade(1f, 0.5f).SetEase(Ease.Linear).SetUpdate(true);
        menuGroup.interactable = true;
        menuGroup.blocksRaycasts = true;
    } 
    #endregion
}

