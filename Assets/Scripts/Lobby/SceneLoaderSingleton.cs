using AYellowpaper.SerializedCollections;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderSingleton : MonoBehaviour
{
    public static SceneLoaderSingleton Instance;

    [SerializedDictionary("Scene Type", "Level Name")]
    [SerializeField] SerializedDictionary<SceneType, string> gameScenes;

    [SerializeField] CanvasGroup loadingPanel;

    private void Awake()
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        } 
        #endregion
    }

    public void LoadGameScene(SceneType sceneType)
    {
        StartCoroutine(LoadGameSceneCR(sceneType));
    }

    private IEnumerator LoadGameSceneCR(SceneType sceneType)
    {
        Tween tween = loadingPanel.DOFade(1f, 0.5f);
        yield return new WaitUntil(() => !tween.IsActive());
        AsyncOperation operation = SceneManager.LoadSceneAsync(gameScenes[sceneType]);
        yield return new WaitUntil(() => operation.isDone);
        loadingPanel.DOFade(0f, 0.5f);
    }

    public string GetScene(SceneType type)
    {
        return gameScenes[type];
    }
}
public enum SceneType
{
    Lobby,
    SinglePlayer_1,
    Multiplayer
}
