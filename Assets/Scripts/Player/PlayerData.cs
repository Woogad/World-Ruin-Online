using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public struct PlayerData : INetworkSerializable, IEquatable<PlayerData>
{
    public ulong ClientID;
    public int PlayerPrefabIndex;

    public bool Equals(PlayerData other)
    {
        return this.ClientID == other.ClientID && this.PlayerPrefabIndex == other.PlayerPrefabIndex;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref ClientID);
        serializer.SerializeValue(ref PlayerPrefabIndex);
    }
}
