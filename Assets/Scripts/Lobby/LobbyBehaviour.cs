using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyBehaviour : MonoBehaviour
{
    [SerializeField] Button singlePlayerButton;
    [SerializeField] Button multiplayerButton;
    [SerializeField] Button quitButton;

    private void Awake()
    {
        singlePlayerButton.onClick.AddListener(StartSinglePlayer);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void StartSinglePlayer()
    {
        SceneLoaderSingleton.Instance.LoadGameScene(SceneType.SinglePlayer_1);
    }

}
