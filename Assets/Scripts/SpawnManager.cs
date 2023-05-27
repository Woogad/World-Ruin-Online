using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class SpawnManager : NetworkBehaviour
{
    public static SpawnManager Instance { get; private set; }

    [SerializeField] private List<Transform> SpawnTransform = new List<Transform>();
    private Transform _playerTransform;

    private void Start()
    {
        Instance = this;
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManagerOnClientConnectedCallback;
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
        Debug.Log("hello from RespawnPlayer");
        StartCoroutine(PlayerReSpawnTimer(e.OwnerClientID));
    }

    [ServerRpc(RequireOwnership = false)]
    private void RespawnPlayerServerRpc(ulong clientID)
    {
        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(clientID, out NetworkClient networkClient))
        {
            return;
        }
        if (!networkClient.PlayerObject.TryGetComponent<Player>(out Player player))
        {
            return;
        }

        player.ReSpawn();
        player.SetSpawnPosition(SpawnTransform[(int)clientID].position);
    }

    private void PlayerOnAnyPlayerSpawned(object sender, EventArgs e)
    {
        if (Player.LocalInstance != null)
        {
            Player.LocalInstance.OnDead -= RespawnPlayer;
            Player.LocalInstance.OnDead += RespawnPlayer;
        }
    }

    private void NetworkManagerOnClientConnectedCallback(ulong clientID)
    {
        Debug.Log("Player " + clientID + " is connected");
        TeleportPlayer(clientID);
    }

    private void TeleportPlayer(ulong clientID)
    {
        if (!IsServer) return;

        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(clientID, out NetworkClient networkClient))
        {
            return;
        }
        if (!networkClient.PlayerObject.TryGetComponent<Player>(out Player player))
        {
            return;
        }
        player.SetSpawnPosition(SpawnTransform[(int)clientID].position);
    }

    private IEnumerator PlayerReSpawnTimer(ulong ClientID)
    {
        float SpawnTime = 4f;
        Debug.Log("Spawn in 4s");
        yield return new WaitForSeconds(SpawnTime);
        RespawnPlayerServerRpc(ClientID);
    }
}
