using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
using System;

public class RankingboardBehaviour : NetworkBehaviour
{
    [SerializeField] private RecordUI _recordPrefab;
    [SerializeField] private RectTransform _recordParentTrm;

    private NetworkList<RankingboardEntityState> _rankList;

    private List<RecordUI> _rankUIList = new();

    private void Awake()
    {
        _rankList = new NetworkList<RankingboardEntityState>();

    }

    public override void OnNetworkSpawn()
    {
        if(IsClient)
        {
            _rankList.OnListChanged += HandleRankListChanged;
            // 맨 처음 접속시 리스트에 있는 모든 애들을 추가하는 작업 실행
            foreach(var data in _rankList)
            {
                RecordUI recordUI = Instantiate(_recordPrefab, _recordParentTrm);
                recordUI.SetOwner(data.clientID);
                recordUI.SetText(0, data.playerName.ToString(), data.score);
                _rankUIList.Add(recordUI);
            }
        }

        if(IsServer)
        {
            ServerSingleton.Instance.NetServer.OnUserJoin += HandleUserJoin;
            ServerSingleton.Instance.NetServer.OnUserLeft += HandleUserLeft;
        }
    }

    public override void OnNetworkDespawn()
    {
        if(IsClient)
        {
            _rankList.OnListChanged -= HandleRankListChanged;
        }

        if (IsServer)
        {
            ServerSingleton.Instance.NetServer.OnUserJoin -= HandleUserJoin;
            ServerSingleton.Instance.NetServer.OnUserLeft -= HandleUserLeft;
        }
    }

    private void HandleUserJoin(ulong clientID, UserData userData)
    {
        // 랭킹보드 추가
        RankingboardEntityState data = new RankingboardEntityState
        {
            clientID = clientID,
            playerName = userData.username,
            score = 0
        };

        _rankList.Add(data);
    }

    private void HandleUserLeft(ulong clientID, UserData userData)
    {
        // 랭킹보드 제거
        foreach(var data in _rankList)
        {
            if(data.clientID == clientID)
            {
                _rankList.Remove(data);
                break;
            }
        }
    }

    private void HandleRankListChanged(NetworkListEvent<RankingboardEntityState> evt)
    {
        switch (evt.Type)
        {
            case NetworkListEvent<RankingboardEntityState>.EventType.Add:
                AddUIToList(evt.Value);
                break;
            case NetworkListEvent<RankingboardEntityState>.EventType.Remove:
                RemoveFromUIList(evt.Value.clientID);
                break;
            case NetworkListEvent<RankingboardEntityState>.EventType.Value:
                break;
        }
    }
    
    private void AddUIToList(RankingboardEntityState value)
    {
        foreach(var data in _rankUIList)
        {
            if (data.clientID == value.clientID)
                return;
        }

        RecordUI recordUI = Instantiate(_recordPrefab, _recordParentTrm);
        recordUI.SetOwner(value.clientID);
        recordUI.SetText(0 , value.playerName.ToString(), value.score);
        _rankUIList.Add(recordUI);
    }

    private void RemoveFromUIList(ulong clientID)
    {
        foreach (var data in _rankUIList)
        {
            if (data.clientID == clientID)
            {
                _rankUIList.Remove(data);
                Destroy(data.gameObject);
                return;
            }
        }
    }

}
