using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PrefabSelectorUI : MonoBehaviour
{
    [SerializeField] Button _increaseBn;
    [SerializeField] Button _decreaseBn;
    [SerializeField] Button _MainMenuBn;
    [SerializeField] TextMeshProUGUI _prefabNumberText;

    private void Awake()
    {
        _increaseBn.onClick.AddListener(() =>
        {
            PlayerPrefabManager.Instance.IncreaseIndex();
        });
        _decreaseBn.onClick.AddListener(() =>
        {
            PlayerPrefabManager.Instance.DecreaseIndex();
        });
        _MainMenuBn.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
    }

    private void Start()
    {
        PlayerPrefabManager.Instance.OnPlayerIndexChanged += PlayerPrefabManagerOnPlayerIndexChanged;
        UpdateVisual();
    }

    private void PlayerPrefabManagerOnPlayerIndexChanged(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        _prefabNumberText.text = (PlayerPrefabManager.Instance.GetPlayerPrefabIndex() + 1).ToString();
    }
}
