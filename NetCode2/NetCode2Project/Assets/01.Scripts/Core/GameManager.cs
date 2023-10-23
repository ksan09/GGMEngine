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

    private void Awake()
    {
        Instance = this; // 굳은 믿음 ( 문제 안 발생할 거임 )
        players = new NetworkList<GameData>();
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
            }catch
            {
                Debug.LogError($"{data.playerName} 삭제중 오류 발생");
            }
            break;
        }
    }


}
