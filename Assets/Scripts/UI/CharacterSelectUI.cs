using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] private Button _mainMenuBn;
    [SerializeField] private Button _readyBn;

    private void Awake()
    {
        _mainMenuBn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        _readyBn.onClick.AddListener(() =>
        {
            ChareacterSelectReady.Instance.SetPlayerReady();
        });
    }
}
