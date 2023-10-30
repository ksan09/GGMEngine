using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public enum GameState
{
    Ready,
    Game,
    Win,
    Lose
}

public enum GameRole : ushort
{
    Host,
    Client
}

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;
    public event Action<GameState> GameStateChanged;
    // ������ ���°� ������ �� �����
    // ������ ������
    private GameState _gameState; // ���� ���� ����

    [SerializeField] private Transform _spawnPosition;
    public Color[] slimeColors; // �������� �÷�

    public NetworkList<GameData> players;
    public GameRole myGameRole;

    private ushort _colorIdx = 0;
    private int _readyUserCount = 0;

    public EggManager EggManager { get; private set; }
    public TurnManager TurnManager { get; private set; }

    private void Awake()
    {
        Instance = this; // ���� ���� ( ���� �� �߻��� ���� )
        players = new NetworkList<GameData>();

        EggManager = GetComponent<EggManager>();
        TurnManager = GetComponent<TurnManager>();
    }

    //�������� ���� ����ɲ���.
    //https://docs-multiplayer.unity3d.com/netcode/current/basics/networkbehavior/
    private void Start()
    {
        _gameState = GameState.Ready;
    }

    public override void OnNetworkSpawn()
    {
        if(IsHost)
        {
            HostSingleton.Instance.GameManager.OnPlayerConnect      += OnPlayerConnectHandle;
            HostSingleton.Instance.GameManager.OnPlayerDisconnect   += OnPlayerDisConnectHandle;

            var gameData = HostSingleton.Instance.GameManager.NetServer.GetUserDataByClientID(OwnerClientId);
            OnPlayerConnectHandle(gameData.userAuthID, OwnerClientId);
            myGameRole = GameRole.Host;
        }
        else
        {
            myGameRole = GameRole.Client;
        }
    }

    public override void OnNetworkDespawn()
    {
        if(IsHost)
        {
            HostSingleton.Instance.GameManager.OnPlayerConnect      -= OnPlayerConnectHandle;
            HostSingleton.Instance.GameManager.OnPlayerDisconnect   -= OnPlayerDisConnectHandle;
        }
    }

    private void OnPlayerConnectHandle(string authID, ulong clientID)
    {
        UserData data = 
            HostSingleton.Instance.GameManager.NetServer.GetUserDataByClientID(clientID);
        players.Add(new GameData
        {
            clientID = clientID,
            playerName = data.name,
            ready = false,
            colorIdx = _colorIdx,
        });
        ++_colorIdx;
    }

    private void OnPlayerDisConnectHandle(string authID, ulong clientID)
    {
        foreach(var data in players)
        {
            if (data.clientID != clientID) continue;

            try
            {
                players.Remove(data);
                --_colorIdx;
            }catch
            {
                Debug.LogError($"{data.playerName} ������ ���� �߻�");
            }
            break;
        }
    }

    public void GameStart()
    {
        if (!IsHost) return;
        if (_readyUserCount == 2 || true)
        {
            // ���⼭ �÷��̾� �� ������� ������ ������ �Բ�
            SpawnPlayers();
            StartGameClientRpc();
        }
        else
        {
            Debug.Log("���� �÷��̾���� �غ���� �ʾҽ��ϴ�.");
        }
    }

    public void GameReady()
    {
        // �� Ŭ�� ���̵� ���� ������
        SendReadyStateServerRpc(NetworkManager.Singleton.LocalClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SendReadyStateServerRpc(ulong clientID)
    {
        //�� Ŭ���̾�Ʈ ���̵� ���� �÷��̾ ã�Ƽ�
        //���� ���¸� ������ֱ�
        //�� ���� �׳� �ٲ��� �ʰ�, 
        //�װ��� �ε����� ã�Ƽ� �ű�ٰ� ���ο� ��ü�� �־��ֱ� ( new GameData )
        //Value Event�� ����

        for (int i = 0; i < players.Count; ++i)
        {
            var oldData = players[i];
            if (oldData.clientID != clientID) continue;

            players[i] = new GameData
            {
                clientID = clientID,
                playerName = oldData.playerName,
                ready = !oldData.ready,
                colorIdx = oldData.colorIdx,
            };

            _readyUserCount += players[i].ready ? 1 : -1;
            break;
        }
    }

    private void SpawnPlayers()
    {
        foreach(var player in players)
        {
            HostSingleton.Instance.GameManager.NetServer.SpawnPlayer(
                player.clientID,
                _spawnPosition.position,
                player.colorIdx);
        }
    }

    [ClientRpc]
    private void StartGameClientRpc()
    {
        _gameState = GameState.Game;
        GameStateChanged?.Invoke(_gameState);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        players?.Dispose();
    }
}
