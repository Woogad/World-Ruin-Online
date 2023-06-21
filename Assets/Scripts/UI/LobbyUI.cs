using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Lobbies.Models;
using TMPro;
using System;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private Button _mainMenuBn;
    [SerializeField] private Button _createLobbyBn;
    [SerializeField] private Button _quickJoinBn;
    [SerializeField] private LobbyCreateUI _lobbyCreateUI;
    [SerializeField] private Button _joinCode;
    [SerializeField] private TMP_InputField _lobbyCodeInput;
    [SerializeField] private TMP_InputField _playerNameInput;
    [SerializeField] private Transform _lobbyContainer;
    [SerializeField] private Transform _lobbyTemplate;
    [SerializeField] private TextMeshProUGUI _lobbyCountText;
    [SerializeField] private Button _pasteBn;

    private void Awake()
    {
        int nameCharacterLimit = 10;
        _playerNameInput.characterLimit = nameCharacterLimit;

        int joinByCodeCharacterLimit = 6;
        _lobbyCodeInput.characterLimit = joinByCodeCharacterLimit;

        _mainMenuBn.onClick.AddListener(() =>
        {
            LobbyManager.Instance.LeaveLobby();
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        _createLobbyBn.onClick.AddListener(() =>
        {
            _lobbyCreateUI.Show();
        });
        _quickJoinBn.onClick.AddListener(() =>
        {
            LobbyManager.Instance.QuickJoin();
        });
        _joinCode.onClick.AddListener(() =>
        {
            LobbyManager.Instance.JoinByCode(_lobbyCodeInput.text);
        });
        _pasteBn.onClick.AddListener(() =>
        {
            TextEditor textEditor = new TextEditor();
            textEditor.Paste();
            _lobbyCodeInput.text = textEditor.text;
        });

        _lobbyTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        _playerNameInput.text = GameMultiplayer.Instance.GetPlayerName();

        _playerNameInput.onValueChanged.AddListener((string newText) =>
        {
            GameMultiplayer.Instance.SetPlayerName(newText);
        });
        LobbyManager.Instance.OnLobbiesListChanged += LobbyManagerOnlobbiesListChanged;
        UpdateLobbyList(new List<Lobby>());
    }

    private void OnDestroy()
    {
        LobbyManager.Instance.OnLobbiesListChanged -= LobbyManagerOnlobbiesListChanged;
    }

    private void LobbyManagerOnlobbiesListChanged(object sender, LobbyManager.OnLobbiesListChangedArgs e)
    {
        UpdateLobbyList(e.LobbyList);
    }

    private void UpdateLobbyList(List<Lobby> lobbyList)
    {
        foreach (Transform child in _lobbyContainer)
        {
            if (child == _lobbyTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (Lobby lobby in lobbyList)
        {
            Transform lobbyTransform = Instantiate(_lobbyTemplate, _lobbyContainer);
            lobbyTransform.gameObject.SetActive(true);
            lobbyTransform.GetComponent<LobbyItemUI>().SetLobby(lobby);
        }

        _lobbyCountText.text = "Lobby Public: " + lobbyList.Count.ToString();
    }
}
