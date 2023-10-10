using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class HostGameManager : IDisposable
{
    public NetworkServer NetServer { get; private set; }
    private Allocation _allocation; //릴레이 서버 방을 만드는 할당정보
    private string _joinCode;
    private string _lobbyId;
    private const int _maxConnections = 2; // 1:1게임

    // string   = authId
    // ulong    = clientId
    private event Action<string, ulong> OnPlayerConnect;
    private event Action<string, ulong> OnPlayerDisconnect;

    private NetworkObject _playerPrefab;
    public HostGameManager(NetworkObject playerPrefab)
    {
        _playerPrefab = playerPrefab;
    }

    public async Task<bool> StartHostAsync(string lobbyName, UserData userData)
    {
        try
        {
            //2명이 들어갈 수 있는 릴레이 서비스 할당받는다.
            _allocation = await Relay.Instance.CreateAllocationAsync(_maxConnections);
            //할당 받은 후에 할당으로부터 조인 코드를 알아낸다.
            _joinCode = await Relay.Instance.GetJoinCodeAsync(_allocation.AllocationId);

            //릴레이 연결을 위해서 트랜스포트를 다시 설정해야 한다.
            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            var relayServerData = new RelayServerData(_allocation, "dtls");

            transport.SetRelayServerData(relayServerData);

            // 여기에는 로비를 만드는 로직이 들어간다.

            // 여기에는 NetworkServer를 만드는 기능이 들어가야 돼.
            NetServer = new NetworkServer(NetworkManager.Singleton, _playerPrefab);
            NetServer.OnClientJoin += HandleClientJoin;
            NetServer.OnClientLeft += HandleClientLeft;

            string userJson = JsonUtility.ToJson(userData);
            NetworkManager.Singleton.NetworkConfig.ConnectionData = 
                                    Encoding.UTF8.GetBytes(userJson);

            NetworkManager.Singleton.StartHost();

            return true;


        }catch(Exception ex)
        {
            Debug.LogError(ex.Message);
            return false;
        }
    }

    private void HandleClientLeft(string authID, ulong clientID)
    {
        OnPlayerDisconnect?.Invoke(authID, clientID);
        //로비에서 빼줘야함 - 아직 로비 없음
    }

    private void HandleClientJoin(string authID, ulong clientID)
    {
        OnPlayerConnect?.Invoke(authID, clientID);
    }

    public void Dispose()
    {
        //리소스 해제
        ShutdownAsync();
    }

    public async void ShutdownAsync()
    {
        //로비 정리 필요하고


        NetServer.OnClientLeft -= HandleClientLeft;
        NetServer.OnClientJoin -= HandleClientJoin;
        _lobbyId = string.Empty;
        NetServer?.Dispose();
        
    }
}
