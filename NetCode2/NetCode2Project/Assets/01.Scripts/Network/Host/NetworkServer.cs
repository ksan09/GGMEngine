using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Netcode;
using UnityEngine;

public class NetworkServer : IDisposable
{
    private NetworkManager _networkManager;
    public Action<string, ulong> OnClientJoin;
    public Action<string, ulong> OnClientLeft;

    // string   = authId
    // ulong    = clientId
    private Dictionary<ulong, string> _clientToAuthDictinary            = new();
    private Dictionary<string, UserData> _authIdToUserDataDictinary     = new();

    private NetworkObject _playerPrefab;
    private List<NetworkObject> _playerList                                = new();

    public NetworkServer(NetworkManager networkManager, NetworkObject playerPrefab)
    {
        _networkManager = networkManager;
        _playerPrefab = playerPrefab;
        _networkManager.ConnectionApprovalCallback += ApprovalCheck;
        //�̳༮�� ���� ��Ʈ��ũ �Ŵ����� �����ϴ� �̺�Ʈ��.
        _networkManager.OnServerStarted += OnServerReady;
    }

    // ���� üũ ����
    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest req, 
                                NetworkManager.ConnectionApprovalResponse res)
    {
        string json = Encoding.UTF8.GetString(req.Payload);
        UserData userData = JsonUtility.FromJson<UserData>(json);

        // Ŭ���̾�Ʈ ���̵� �̿��ؼ� authID
        _clientToAuthDictinary[req.ClientNetworkId] = userData.userAuthID;
        _authIdToUserDataDictinary[userData.userAuthID] = userData;

        res.Approved = true;
        res.CreatePlayerObject = false;

        OnClientJoin?.Invoke(userData.userAuthID, req.ClientNetworkId);
    }

    private void OnServerReady()
    {
        _networkManager.OnClientDisconnectCallback += OnClientDisconnect;
    }

    // Ŭ�� ���� ���� �� ���� ���� ����ٰ� ����
    private void OnClientDisconnect(ulong clientID)
    {
        if(_clientToAuthDictinary.TryGetValue(clientID, out var authID))
        {
            _clientToAuthDictinary.Remove(clientID);
            _authIdToUserDataDictinary.Remove(authID);
            OnClientLeft?.Invoke(authID, clientID);
        }
    }

    public UserData GetUserDataByClientID(ulong clientID)
    {
        if(_clientToAuthDictinary.TryGetValue(clientID, out string authID))
            if(_authIdToUserDataDictinary.TryGetValue(authID, out UserData data))
                return data;

        return null;
    }
    public UserData GetUserDataByAuthID(string authID)
    {
        if(_authIdToUserDataDictinary.TryGetValue(authID, out UserData data))
            return data;

        return null;
    }

    public void Dispose()
    {
        if (_networkManager == null) return; //����Ƽ Ŭ�� �������

        _networkManager.ConnectionApprovalCallback -= ApprovalCheck;
        _networkManager.OnServerStarted -= OnServerReady;
        _networkManager.OnClientDisconnectCallback -= OnClientDisconnect;

        if(_networkManager.IsListening)
        {
            _networkManager.Shutdown(); // ����
        }
    }

    public void SpawnPlayer(ulong clientID, Vector3 position, ushort colorIdx)
    {
        var player = GameObject.Instantiate(_playerPrefab, position, Quaternion.identity);
        player.SpawnAsPlayerObject(clientID);
        _playerList.Add(player);

        if(player.TryGetComponent<PlayerColorizer>(out PlayerColorizer playerColorizer))
        {
            playerColorizer.SetColor(colorIdx);

            var controller = player.GetComponent<PlayerStateController>();
            controller.InitStateClientRPC(clientID == NetworkManager.Singleton.LocalClientId);
        }
    }

    public void DestroyAllPlayer()
    {
        foreach(var p in _playerList)
        {
            GameObject.Destroy(p.gameObject);
        }
        _playerList.Clear();
    }
}
