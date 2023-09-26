using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class GameHud : NetworkBehaviour
{
    [SerializeField] private Color _ownerColor;
    [SerializeField] private VisualTreeAsset _boardItemAsset;
    [SerializeField] private int _displayCount = 7;

    private LeaderBoard _leaderBoard;
    private NetworkList<LeaderboardEntityState> _leaderBoardEntites; //�������� ��ƼƼ�� �ִ°�

    private UIDocument _document;
    private VisualElement _msgContainer;
    private Label _msgLabel;

    private void Awake()
    {
        _leaderBoardEntites = new NetworkList<LeaderboardEntityState>();
        _document = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        var root = _document.rootVisualElement;
        var boardContainer = root.Q<VisualElement>("leaderboard");
        _leaderBoard = new LeaderBoard(boardContainer, _boardItemAsset, _ownerColor, 
                                        _displayCount);
        _msgContainer = root.Q<VisualElement>("msg-container");
        _msgLabel = root.Q<Label>("msg-label");
    }

    public override void OnNetworkSpawn()
    {
        if(IsClient)
        {
            _leaderBoardEntites.OnListChanged += HandleLeaderboardChanged;

            foreach (var ent in _leaderBoardEntites)
            {
                HandleLeaderboardChanged(new NetworkListEvent<LeaderboardEntityState>
                {
                    Type = NetworkListEvent<LeaderboardEntityState>.EventType.Add,
                    Value = ent
                });
            }
        }

        if(IsServer)
        {
            TankPlayer[] players = FindObjectsByType<TankPlayer>(FindObjectsSortMode.None);
            foreach(TankPlayer player in players)
            {
                HandlePlayerSpawned(player);
            }

            TankPlayer.OnPlayerSpawned += HandlePlayerSpawned;
            TankPlayer.OnPlayerDespawned += HandlePlayerDespawned;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsClient)
        {
            _leaderBoardEntites.OnListChanged -= HandleLeaderboardChanged;
        }

        if (IsServer)
        {
            TankPlayer.OnPlayerSpawned -= HandlePlayerSpawned;
            TankPlayer.OnPlayerDespawned -= HandlePlayerDespawned;
        }
    }

    private void HandlePlayerSpawned(TankPlayer player)
    {
        _leaderBoardEntites.Add(new LeaderboardEntityState
        {
            clientID    = player.OwnerClientId,
            playerName  = player.playerName.Value,
            coins       = 0
        });

        player.Coin.totalCoins.OnValueChanged += (oldCoin, newCoin) =>
        {
            HandleCoinsChanged(player.OwnerClientId, newCoin);
        };
    }

    private void HandleCoinsChanged(ulong ownerClientId, int newCoin)
    {
        for(int i = 0; i < _leaderBoardEntites.Count; ++i)
        {
            if (_leaderBoardEntites[i].clientID != ownerClientId)
                continue;

            var oldItem = _leaderBoardEntites[i];
            _leaderBoardEntites[i] = new LeaderboardEntityState
            {
                clientID = oldItem.clientID,
                playerName = oldItem.playerName,
                coins = newCoin
            };
            break;
        }
    }

    private void HandlePlayerDespawned(TankPlayer player)
    {
        if (_leaderBoardEntites == null) return;
        // ��ũ�� ������� �������� �̹� �� ������Ʈ�� ������ ���� �ִ�.
        // ������ �����ٸ� ������

        foreach(var ent in _leaderBoardEntites)
        {
            if(ent.clientID != player.OwnerClientId) continue;

            try
            {
                _leaderBoardEntites.Remove(ent);
            } catch(Exception e)
            {
                Debug.LogWarning($"{ent.playerName}-{ent.clientID} ������ ����");
            }
            break;
        }

        player.Coin.totalCoins.OnValueChanged = null;
    }

    private void HandleLeaderboardChanged(NetworkListEvent<LeaderboardEntityState> evt)
    {
        switch (evt.Type)
        {
            case NetworkListEvent<LeaderboardEntityState>.EventType.Add:
                //���� ����Ʈ�� ���� �߰��� �༮�� �������� ���� ��
                if(!_leaderBoard.CheckExistByClientID(evt.Value.clientID))
                {
                    _leaderBoard.AddItem(evt.Value);
                }
                break;
            case NetworkListEvent<LeaderboardEntityState>.EventType.Remove:
                _leaderBoard.RemoveByClientID(evt.Value.clientID);
                break;
            case NetworkListEvent<LeaderboardEntityState>.EventType.Value:
                _leaderBoard.UpdateByClientID(evt.Value.clientID, evt.Value.coins);
                break;
            default:
                break;
        }

        _leaderBoard.SortOrder();
    }

    public void ShowMsgContainer(bool show, string text = "")
    {
        if (show)
        {
            _msgContainer.RemoveFromClassList("off");
            _msgLabel.text = text;
        }
        else
            _msgContainer.AddToClassList("off");
    }
}
