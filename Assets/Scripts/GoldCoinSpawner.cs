using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class GoldCoinSpawner : NetworkBehaviour
{
    [SerializeField] private Vector2 _areaSpawn;
    [SerializeField] private float _spawnTimerMax;
    private NetworkVariable<float> _spawnTimer = new NetworkVariable<float>();


    private void Update()
    {
        if (!IsServer) return;
        if (GameManager.Instance.IsGamePlaying())
        {
            _spawnTimer.Value -= Time.deltaTime;
            if (_spawnTimer.Value <= 0)
            {
                _spawnTimer.Value = _spawnTimerMax;
                GameMultiplayer.Instance.SpawnGoldCoinObject(gameObject.transform.position, _areaSpawn);
            }
        }
    }
}
