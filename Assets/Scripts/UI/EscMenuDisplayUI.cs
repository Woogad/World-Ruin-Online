using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;

public class EscMenuDisplayUI : MonoBehaviour
{
    [SerializeField] private Button _mainMenuBn;
    [SerializeField] private Button _optionsBn;
    [SerializeField] private OptionsUI _optionsUI;

    private void Awake()
    {
        _mainMenuBn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        _optionsBn.onClick.AddListener(() =>
        {
            _optionsUI.Show();
        });
    }
    private void Start()
    {
        GameInput.Instance.OnEscAction += GameInputOnEscAction;
        Hide();
    }

    private void GameInputOnEscAction(object sender, GameInput.OnEscActionArgs e)
    {
        if (e.IsEscMenuOpen)
        {
            Show();
        }
        else
        {
            _optionsUI.Hide();
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
