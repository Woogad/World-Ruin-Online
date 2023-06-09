using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Netcode;

public struct PlayerData : INetworkSerializable, IEquatable<PlayerData>
{
    public ulong ClientID;
    public int PlayerPrefabIndex;
    public FixedString64Bytes PlayerName;
    public FixedString64Bytes PlayerID;

    public bool Equals(PlayerData other)
    {
        return this.ClientID == other.ClientID &&
        this.PlayerPrefabIndex == other.PlayerPrefabIndex &&
        this.PlayerName == other.PlayerName &&
        this.PlayerID == other.PlayerID;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref ClientID);
        serializer.SerializeValue(ref PlayerPrefabIndex);
        serializer.SerializeValue(ref PlayerName);
        serializer.SerializeValue(ref PlayerID);
    }
}
