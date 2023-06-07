using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button _playBn;
    [SerializeField] private Button _quitBn;
    [SerializeField] private Button _profileBn;

    private void Awake()
    {
        _playBn.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.LobbyScene);
        });
        _profileBn.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.ProfileScene);
        });
        _quitBn.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
