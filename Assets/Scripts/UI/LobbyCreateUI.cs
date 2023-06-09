using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyCreateUI : MonoBehaviour
{
    [SerializeField] Button _closeBn;
    [SerializeField] Button _createPublicBn;
    [SerializeField] Button _createPrivateBn;
    [SerializeField] TMP_InputField _lobbyNameInput;

    private void Awake()
    {
        _closeBn.onClick.AddListener(() =>
        {
            Hide();
        });
        _createPublicBn.onClick.AddListener(() =>
        {
            LobbyManager.Instance.CreateLobby(_lobbyNameInput.text, false);
        });
        _createPrivateBn.onClick.AddListener(() =>
        {
            LobbyManager.Instance.CreateLobby(_lobbyNameInput.text, true);
        });
    }

    private void Start()
    {
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
