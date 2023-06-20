using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public struct PlayerReady : INetworkSerializable, IEquatable<PlayerReady>
{
    public ulong ClientID;
    public bool IsReady;

    public bool Equals(PlayerReady other)
    {
        return this.ClientID == other.ClientID && this.IsReady == other.IsReady;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref ClientID);
        serializer.SerializeValue(ref IsReady);
    }


}


