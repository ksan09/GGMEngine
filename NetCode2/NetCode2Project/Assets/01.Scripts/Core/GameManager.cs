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
    // 게임의 상태가 변했을 때 발행됨
    // 서버만 실행함
    private GameState _gameState; // 현재 게임 상태

    [SerializeField] private Transform _spawnPosition;
    public Color[] slimeColors; // 슬라임의 컬러

    public NetworkList<GameData> players;
    public GameRole myGameRole;

    private ushort _colorIdx = 0;
    private int _readyUserCount = 0;

    public EggManager EggManager { get; private set; }
    public TurnManager TurnManager { get; private set; }

    private void Awake()
    {
        Instance = this; // 굳은 믿음 ( 문제 안 발생할 거임 )
        players = new NetworkList<GameData>();

        EggManager = GetComponent<EggManager>();
        TurnManager = GetComponent<TurnManager>();
    }

    //스폰보다 먼저 실행될꺼다.
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
                Debug.LogError($"{data.playerName} 삭제중 오류 발생");
            }
            break;
        }
    }

    public void GameStart()
    {
        if (!IsHost) return;
        if (_readyUserCount == 2 || true)
        {
            // 여기서 플레이어 턴 기반으로 돌리는 로직도 함께
            SpawnPlayers();
            StartGameClientRpc();
        }
        else
        {
            Debug.Log("아직 플레이어들이 준비되지 않았습니다.");
        }
    }

    public void GameReady()
    {
        // 내 클라 아이디 레디 보내기
        SendReadyStateServerRpc(NetworkManager.Singleton.LocalClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SendReadyStateServerRpc(ulong clientID)
    {
        //이 클라이언트 아이디를 가진 플레이어를 찾아서
        //레디 상태를 토글해주기
        //그 값을 그냥 바꾸지 않고, 
        //그것의 인덱스를 찾아서 거기다가 새로운 객체로 넣어주기 ( new GameData )
        //Value Event를 위해

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
