using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TestingLobbyUI : MonoBehaviour
{
    [SerializeField] Button _createGameBn;
    [SerializeField] Button _joinGameBn;

    private void Awake()
    {
        _createGameBn.onClick.AddListener(() =>
        {
            GameMultiplayer.Instance.StartHost();
            Loader.LoadNetwork(Loader.Scene.CharacterSelectScene);
        });
        _joinGameBn.onClick.AddListener(() =>
        {
            GameMultiplayer.Instance.StartClient();
        });
    }
}
