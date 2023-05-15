using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EscMenuDisplayUI : MonoBehaviour
{
    [SerializeField] private Button _mainMenuBn;

    private void Awake()
    {
        _mainMenuBn.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
    }
    private void Start()
    {
        GameInput.Instance.OnEscAction += GmaeInputOnEscAction;
        Hide();
    }

    private void GmaeInputOnEscAction(object sender, GameInput.OnEscActionArgs e)
    {
        if (e.IsEscMenuOpen)
        {
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
