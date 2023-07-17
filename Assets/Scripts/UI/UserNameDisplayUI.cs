using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserNameDisplayUI : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private TextMeshPro _userNameText;

    private int _playerIndex;

    private void Start()
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (_player.IsOwner)
        {
            Hide();
        }
        else
        {
            foreach (var player in GameMultiplayer.Instance.GetPlayerDataNetworkList())
            {
                if (player.ClientID == _player.OwnerClientId)
                {
                    _userNameText.text = player.PlayerName.ToString();
                }
            }
            Show();
        }
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
