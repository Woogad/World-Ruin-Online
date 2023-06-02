using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;

public class ConnectionResponseMessagesUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _messagesText;
    [SerializeField] private Button _closeBn;

    private void Awake()
    {
        _closeBn.onClick.AddListener(Hide);
    }

    private void Start()
    {
        GameMultiplayer.Instance.OnFailToJoinGame += GameMultiplayerOnFailToJoinGame;
        Hide();
    }

    private void OnDestroy()
    {
        GameMultiplayer.Instance.OnFailToJoinGame -= GameMultiplayerOnFailToJoinGame;
    }

    private void GameMultiplayerOnFailToJoinGame(object sender, EventArgs e)
    {
        _messagesText.text = NetworkManager.Singleton.DisconnectReason;
        if (_messagesText.text == "")
        {
            _messagesText.text = "Fail to connect";
        }
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
