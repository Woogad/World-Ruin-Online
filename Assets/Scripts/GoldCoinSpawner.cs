using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class GoldCoinSpawner : NetworkBehaviour
{
    [SerializeField] private GoldCoinSO _goldCoinSO;
    [SerializeField] private Vector3 _areaSpawn;
    private NetworkVariable<float> _spawnTimer = new NetworkVariable<float>();
    private float _spawnTimerMax = 10f;


    private void Update()
    {
        if (!IsServer) return;
        if (GameManager.Instance.IsGamePlaying())
        {
            _spawnTimer.Value -= Time.deltaTime;
            if (_spawnTimer.Value <= 0)
            {
                _spawnTimer.Value = _spawnTimerMax;
                //TODO Add Spawn within Area later
                Transform goldCoinTransform = Instantiate(_goldCoinSO.Prefab, new Vector3(0, 1, 0), Quaternion.identity);
                NetworkObject goldCoinNetworkObject = goldCoinTransform.GetComponent<NetworkObject>();
                goldCoinNetworkObject.Spawn(true);
                Debug.Log("Spawn Gold Coin!");
            }
        }
    }
}
