using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;

public class PlayerLobbyRoomVisual : MonoBehaviour
{
    [SerializeField] private int _playerIndex;
    [SerializeField] private GameObject _readyText;
    [SerializeField] private PlayerPrefabsVisualListSO _playerPrefabsVisualListSO;
    [SerializeField] private List<Transform> _prefabList = new List<Transform>();
    [SerializeField] private Button _kickBn;
    [SerializeField] private TextMeshPro _playerNameText;

    private int _currentPrefabIndex;
    private int _oldPrefabIndex;

    private void Awake()
    {
        _kickBn.onClick.AddListener(() =>
        {
            PlayerData playerData = GameMultiplayer.Instance.GetPlayerDataFormPlayerIndex(_playerIndex);
            LobbyManager.Instance.KickPlayer(playerData.PlayerID.ToString());
            GameMultiplayer.Instance.KickPlayer(playerData.ClientID);
        });
    }

    private void Start()
    {
        GameMultiplayer.Instance.OnDataNetworkListChanged += GameMultiplayerOnDataNetworkListChanged;
        PlayerSelectReady.Instance.OnReadyChanged += ChareacterSelectReadyOnReadyChanged;
        foreach (Transform prefab in _playerPrefabsVisualListSO.PlayerPrefabVisaulList)
        {
            Transform prefabTrasform = Instantiate(prefab, gameObject.transform);
            prefabTrasform.gameObject.SetActive(false);
            _prefabList.Add(prefabTrasform);
        }

        _kickBn.gameObject.SetActive(NetworkManager.Singleton.IsServer);
        UpdateVisual();
    }

    private void ChareacterSelectReadyOnReadyChanged(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void OnDestroy()
    {
        GameMultiplayer.Instance.OnDataNetworkListChanged -= GameMultiplayerOnDataNetworkListChanged;
    }

    private void GameMultiplayerOnDataNetworkListChanged(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (GameMultiplayer.Instance.IsPlayerDataIndexConnected(_playerIndex))
        {
            PlayerData playerData = GameMultiplayer.Instance.GetPlayerDataFormPlayerIndex(_playerIndex);
            _readyText.SetActive(PlayerSelectReady.Instance.IsPlayerReady(playerData.ClientID));
            _playerNameText.text = playerData.PlayerName.ToString();

            _currentPrefabIndex = playerData.PlayerPrefabIndex;

            if (_currentPrefabIndex != _oldPrefabIndex)
            {
                _prefabList[_oldPrefabIndex].gameObject.SetActive(false);
                _prefabList[_currentPrefabIndex].gameObject.SetActive(true);
                _oldPrefabIndex = _currentPrefabIndex;
            }
            else
            {
                _prefabList[_currentPrefabIndex].gameObject.SetActive(true);
            }
            Show();
        }
        else
        {
            Hide();
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
