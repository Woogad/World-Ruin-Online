using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectingUI : MonoBehaviour
{
    private void Start()
    {
        GameMultiplayer.Instance.OnTryToJoinGame += GameMultiplayerOnTryToJoinGame;
        GameMultiplayer.Instance.OnFailToJoinGame += GameMultiplayerOnFailToJoinGame;
        Hide();
    }

    private void OnDestroy()
    {
        GameMultiplayer.Instance.OnTryToJoinGame -= GameMultiplayerOnTryToJoinGame;
        GameMultiplayer.Instance.OnFailToJoinGame -= GameMultiplayerOnFailToJoinGame;
    }

    private void GameMultiplayerOnFailToJoinGame(object sender, EventArgs e)
    {
        Hide();
    }

    private void GameMultiplayerOnTryToJoinGame(object sender, EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
