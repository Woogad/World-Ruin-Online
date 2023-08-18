using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button _playBn;
    [SerializeField] private Button _quitBn;
    [SerializeField] private Button _profileBn;
    [SerializeField] private Button _optionsBn;
    [SerializeField] private OptionsUI _optionsUI;
    [SerializeField] private HowToPlayUI _howToPlayUI;
    [SerializeField] private Button _howToPlayBn;
    [SerializeField] private TextMeshProUGUI _versionText;
    [SerializeField] private Button _creditBn;
    [SerializeField] private GameObject _creditUI;

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
        _howToPlayBn.onClick.AddListener(() =>
        {
            Debug.Log("hehe");
            _howToPlayUI.Show();
        });
        _optionsBn.onClick.AddListener(() =>
        {
            _optionsUI.Show();
        });
        _quitBn.onClick.AddListener(() =>
        {
            Application.Quit();
        });
        _creditBn.onClick.AddListener(() =>
        {
            _creditUI.SetActive(true);
        });
    }

    private void Start()
    {
        _creditUI.SetActive(false);
        _versionText.text = "Version " + Application.version;
    }
}
