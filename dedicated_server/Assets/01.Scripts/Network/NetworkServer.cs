using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class UserData
{
    public string username;
}

public class NetworkServer : IDisposable
{
    public delegate void UserChanged(ulong clientID, UserData userData);

    public event UserChanged OnUserJoin;
    public event UserChanged OnUserLeft;

    private NetworkObject _playerPrefab;
    private NetworkManager _networkManager;

    private Dictionary<ulong, UserData> _clientIdToUserDataDictionary = new Dictionary<ulong, UserData>();

    public NetworkServer(NetworkObject playerPrefab)
    {
        _playerPrefab = playerPrefab;
        _networkManager = NetworkManager.Singleton;

        _networkManager.ConnectionApprovalCallback += HandleConnectionApproval;
        _networkManager.OnServerStarted += HandleServerStarted;
    }

    private void HandleConnectionApproval(NetworkManager.ConnectionApprovalRequest req, 
                            NetworkManager.ConnectionApprovalResponse res)
    {
        string json = Encoding.UTF8.GetString(req.Payload);
        UserData userData = JsonUtility.FromJson<UserData>(json);

        _clientIdToUserDataDictionary[req.ClientNetworkId] = userData;

        //4명까지
        res.Approved = (_clientIdToUserDataDictionary.Count <= 4);
        res.CreatePlayerObject = false;

        Debug.Log($"{userData.username} [ {req.ClientNetworkId} ] is logined!");
    }

    private void HandleServerStarted()
    {
        _networkManager.OnClientConnectedCallback += HandleClientConnect;
        _networkManager.OnClientDisconnectCallback += HandleClientDisconnect;
    }

    private void HandleClientConnect(ulong clientID)
    {

        RespawnPlayer(clientID);
        if (_clientIdToUserDataDictionary.TryGetValue(clientID, out var userData))
        {
            OnUserJoin?.Invoke(clientID, userData);
        }
        
    }

    private void HandleClientDisconnect(ulong clientID)
    {
        if(_clientIdToUserDataDictionary.TryGetValue(clientID, out var userData))
        {
            _clientIdToUserDataDictionary.Remove(clientID);
            OnUserLeft?.Invoke(clientID, userData);
        }
    }

    public void RespawnPlayer(ulong clientID)
    {
        // 위쪽 플레이어 스폰 코드를 잘 참조해서 리스폰 해주기
        // 기본 리스폰은 vector3.zero
        NetworkObject player = GameObject.Instantiate(_playerPrefab, Vector3.zero, Quaternion.identity);
        player.SpawnAsPlayerObject(clientID);

        var userData = _clientIdToUserDataDictionary[clientID];

        if (player.TryGetComponent<Player>(out Player playerTemp))
        {
            playerTemp.SetUserName(userData.username);
            Debug.Log($"Create {userData.username} complete!");
        }
        else
        {
            Debug.LogError($"Create {userData.username} failed!");
        }
    }

    public bool OpenConnection(string ipAddress, ushort port)
    {
        UnityTransport transport = _networkManager.GetComponent<UnityTransport>();
        transport.SetConnectionData(ipAddress, port);
        return _networkManager.StartServer();
    }

    public UserData GetUserDataByClientID(ulong clientId)
    {
        if(_clientIdToUserDataDictionary.TryGetValue(clientId, out UserData userData))
        {
            return userData;
        }

        return null;
    }

    public void Dispose()
    {
        if (_networkManager == null) return;

        _networkManager.ConnectionApprovalCallback  -= HandleConnectionApproval;
        _networkManager.OnServerStarted             -= HandleServerStarted;
        _networkManager.OnClientConnectedCallback   -= HandleClientConnect;
        _networkManager.OnClientDisconnectCallback  -= HandleClientDisconnect;

        if (_networkManager.IsListening)    //서버가 리스닝
        {
            _networkManager.Shutdown();
        }
    }
}
