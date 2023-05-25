using System;
using Unity.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Collections;

public struct ScoreBoardStruct : INetworkSerializable
{
    public ScoreBoardStruct(FixedString64Bytes Username, int KillScore = default)
    {
        this.Username = Username;
        this.KillScore = KillScore;
    }

    public FixedString64Bytes Username;
    public int KillScore;

    public void AddScoreKill(int score)
    {
        this.KillScore += score;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref Username);
        serializer.SerializeValue(ref KillScore);
    }
}


