using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public struct RankingboardEntityState : INetworkSerializable, IEquatable<RankingboardEntityState>
{

    public ulong clientID;
    public FixedString32Bytes playerName;
    public int score;

    public bool Equals(RankingboardEntityState other)
    {
        return clientID == other.clientID;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref clientID);
        serializer.SerializeValue(ref playerName);
        serializer.SerializeValue(ref score);
    }
}
