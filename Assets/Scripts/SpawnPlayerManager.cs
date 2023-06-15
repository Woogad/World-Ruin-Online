using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class SpawnPlayerManager : NetworkBehaviour
{
    public static SpawnPlayerManager Instance { get; private set; }

    [SerializeField] private List<Transform> _spawnTransform = new List<Transform>();
    private Transform _playerTransform;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        if (Player.LocalInstance != null)
        {
            Player.LocalInstance.OnDead += RespawnPlayer;
        }
        else
        {
            Player.OnAnyPlayerSpawned += PlayerOnAnyPlayerSpawned;
        }
    }

    private void RespawnPlayer(object sender, Player.OnDeadArgs e)
    {
        StartCoroutine(PlayerReSpawnTimer());
    }

    [ServerRpc(RequireOwnership = false)]
    private void RespawnPlayerServerRpc(ServerRpcParams serverRpcParams = default)
    {
        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(serverRpcParams.Receive.SenderClientId, out NetworkClient networkClient))
        {
            return;
        }
        if (!networkClient.PlayerObject.TryGetComponent<Player>(out Player player))
        {
            return;
        }

        player.ReSpawn();
        player.SetSpawnPosition(_spawnTransform[(int)serverRpcParams.Receive.SenderClientId].position);
    }

    private void PlayerOnAnyPlayerSpawned(object sender, EventArgs e)
    {
        if (Player.LocalInstance != null)
        {
            Player.LocalInstance.OnDead -= RespawnPlayer;
            Player.LocalInstance.OnDead += RespawnPlayer;
        }
    }

    public void TeleportPlayerOnNetworkSpawn(int playerIndex, ulong clientID)
    {
        TeleportPlayerServerRpc(_spawnTransform[playerIndex].position, clientID);
    }

    public void TeleportPlayer(Vector3 position, ulong clientID)
    {
        TeleportPlayerServerRpc(position, clientID);
    }

    [ServerRpc(RequireOwnership = false)]
    private void TeleportPlayerServerRpc(Vector3 position, ulong clientID)
    {

        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(clientID, out NetworkClient networkClient))
        {
            return;
        }
        if (!networkClient.PlayerObject.TryGetComponent<Player>(out Player player))
        {
            return;
        }
        player.SetSpawnPosition(position);
    }

    private IEnumerator PlayerReSpawnTimer()
    {
        float spawnTime = 4f;
        Debug.Log($"Spawn in {spawnTime}");
        yield return new WaitForSeconds(spawnTime);
        RespawnPlayerServerRpc();
    }
}
