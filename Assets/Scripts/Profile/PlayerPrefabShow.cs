using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefabShow : MonoBehaviour
{

    private List<Transform> _playerPrefabList = new List<Transform>();
    private int _currentPrefabIndex;
    private int _oldPrefabIndex;

    private void Start()
    {
        PlayerPrefabManager.Instance.OnPlayerIndexChanged += PlayerPrefabManagerOnPlayerIndexChanged;
        _currentPrefabIndex = PlayerPrefabManager.Instance.GetPlayerPrefabIndex();
        _oldPrefabIndex = PlayerPrefabManager.Instance.GetPlayerPrefabIndex();
        foreach (Transform prefab in PlayerPrefabManager.Instance.GetPlayerPrefabsVisualList())
        {
            Transform prefabTrasform = Instantiate(prefab, gameObject.transform);
            prefabTrasform.gameObject.SetActive(false);
            _playerPrefabList.Add(prefabTrasform);
        }

        UpdatePrefabShow();
    }

    private void PlayerPrefabManagerOnPlayerIndexChanged(object sender, EventArgs e)
    {
        UpdatePrefabShow();
    }

    private void UpdatePrefabShow()
    {
        _currentPrefabIndex = PlayerPrefabManager.Instance.GetPlayerPrefabIndex();
        if (_currentPrefabIndex != _oldPrefabIndex)
        {
            _playerPrefabList[_oldPrefabIndex].gameObject.SetActive(false);
            _playerPrefabList[_currentPrefabIndex].gameObject.SetActive(true);
            _oldPrefabIndex = _currentPrefabIndex;
        }
        else
        {
            _playerPrefabList[_currentPrefabIndex].gameObject.SetActive(true);
        }
    }
}
