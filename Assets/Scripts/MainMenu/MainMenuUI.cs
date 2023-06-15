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
    [SerializeField] private Button _optionsBn;
    [SerializeField] private OptionsUI _optionsDisplay;

    private void Awake()
    {
        _playBn.onClick.AddListener(() =>
        {
            GameMultiplayer.IsPlayMultiplayer = true;
            Loader.Load(Loader.Scene.LobbyScene);
        });
        _profileBn.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.ProfileScene);
        });
        _optionsBn.onClick.AddListener(() =>
        {
            OptionsUI.Instance.Show();
        });
        _quitBn.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
