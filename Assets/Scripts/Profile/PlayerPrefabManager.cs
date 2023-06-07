using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IngameDebugConsole;

public class PlayerPrefabManager : MonoBehaviour
{
    public static PlayerPrefabManager Instance { get; private set; }

    [SerializeField] List<Transform> _playerPrefabVisualList;

    public event EventHandler OnPlayerIndexChanged;

    private const string PLAYER_PREFAB_INDEX = "PlayerPrefabIndex";
    private int _playerPrefabIndex;

    private void Awake()
    {
        Instance = this;
        _playerPrefabIndex = PlayerPrefs.GetInt(PLAYER_PREFAB_INDEX, 0);
    }

    public void IncreaseIndex()
    {
        _playerPrefabIndex += 1;
        if (_playerPrefabIndex >= _playerPrefabVisualList.Count)
        {
            _playerPrefabIndex = 0;
        }
        PlayerPrefs.SetInt(PLAYER_PREFAB_INDEX, _playerPrefabIndex);
        OnPlayerIndexChanged?.Invoke(this, EventArgs.Empty);
    }

    public void DecreaseIndex()
    {
        _playerPrefabIndex -= 1;
        if (_playerPrefabIndex < 0)
        {
            _playerPrefabIndex = _playerPrefabVisualList.Count - 1;
        }
        PlayerPrefs.SetInt(PLAYER_PREFAB_INDEX, _playerPrefabIndex);
        OnPlayerIndexChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SavePlayerPrefab()
    {
        PlayerPrefs.SetInt(PLAYER_PREFAB_INDEX, _playerPrefabIndex);
        PlayerPrefs.Save();
    }

    public int GetPlayerPrefabIndex()
    {
        return this._playerPrefabIndex;
    }

    public List<Transform> GetPlayerPrefabsVisualList()
    {
        return this._playerPrefabVisualList;
    }

}
