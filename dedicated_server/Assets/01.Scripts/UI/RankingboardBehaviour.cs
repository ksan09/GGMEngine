using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
using System;
using UnityEditor.Rendering.Universal;

public class RankingboardBehaviour : NetworkBehaviour
{
    private static RankingboardBehaviour _instance;
    public static RankingboardBehaviour Instance
    {
        get
        {
            if (_instance != null) return _instance;
            _instance = FindObjectOfType<RankingboardBehaviour>();

            if (_instance == null)
            {
                Debug.LogError("Server singleton does not exists");
            }
            return _instance;
        }
    }

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
                HandleRankListChanged(new NetworkListEvent<RankingboardEntityState>
                {
                    Type = NetworkListEvent<RankingboardEntityState>.EventType.Add,
                    Value = data,

                });
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
        _rankList.Add(new RankingboardEntityState
        {
            clientID = clientID,
            playerName = userData.username,
            score = 0
        });
    }

    private void HandleUserLeft(ulong clientID, UserData userData)
    {
        // 랭킹보드 제거
        foreach(var data in _rankList)
        {
            if(data.clientID == clientID)
            {
                try
                {
                    _rankList.Remove(data);
                } catch(Exception ex)
                {
                    Debug.LogError($"{data.playerName} [ {data.clientID} ] : delete error\n{ex.Message}");
                }
                break;
            }
        }
    }

    // 서버가 실행, 클라는 안 건드림
    public void HandleChangeScore(ulong clientID, int score)
    {
        for(int i = 0; i < _rankList.Count; ++i)
        {
            if (_rankList[i].clientID != clientID) continue;

            var oldItem = _rankList[i];
            _rankList[i] = new RankingboardEntityState
            {
                clientID = clientID,
                playerName = oldItem.playerName,
                score = score
            };

            break;
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
                AdjustScoreToUIList(evt.Value);
                break;
        }
    }

    private void AdjustScoreToUIList(RankingboardEntityState value)
    {
        //값을 받아서 해당 UI를 찾아서 ( 올바른 클라이언트 ID ) score를 갱신한다.
        var target = _rankUIList.Find(x => x.clientID == value.clientID);
        if (target == null)
        {
            Debug.LogError("Error adjust score to list");
        }
        else
        {
            target.SetText(1, value.playerName.ToString(), value.score);
        }

        _rankUIList.Sort((a, b) => { return a.Compare(a, b); });
        for(int i = 0; i < _rankUIList.Count; ++i)
        {
            _rankUIList[i].transform.SetParent(null);
            _rankUIList[i].transform.SetParent(_recordParentTrm);
            _rankUIList[i].SetText(i + 1, _rankUIList[i].username, _rankUIList[i].score);
        }
        //target.SetText(0, value.playerName.ToString(), value.score);
        //선택 : 갱신 후에는 UI List를 정렬하고, 정렬된 순서에 맞춰서 실제 UI의 순서도 변경한다.
        
    }

    private void AddUIToList(RankingboardEntityState value)
    {
        var target = _rankUIList.Find(x => x.clientID == value.clientID);
        if (target != null) return;

        RecordUI recordUI = Instantiate(_recordPrefab, _recordParentTrm);
        recordUI.SetOwner(value.clientID);
        recordUI.SetText(1 , value.playerName.ToString(), value.score);
        _rankUIList.Add(recordUI);
    }

    private void RemoveFromUIList(ulong clientID)
    {
        var target = _rankUIList.Find(x => x.clientID == clientID);
        if (target != null)
        {
            _rankUIList.Remove(target);
            Destroy(target.gameObject);
        }
    }

    public void KillPoint(ulong clientID)
    {
        for (int i = 0; i < _rankList.Count; ++i)
        {
            if (_rankList[i].clientID != clientID) continue;

            var oldItem = _rankList[i];
            _rankList[i] = new RankingboardEntityState
            {
                clientID = clientID,
                playerName = oldItem.playerName,
                score = oldItem.score + 10
            };

            break;
        }
    }

}
