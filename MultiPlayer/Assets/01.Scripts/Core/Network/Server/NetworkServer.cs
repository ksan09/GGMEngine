using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkServer : IDisposable
{
    private NetworkManager _networkManager;
    private Dictionary<ulong, string> _clientToAuthDictinary = new Dictionary<ulong, string>();
    private Dictionary<string, UserData> _authToUserDataDictinary = new Dictionary<string, UserData>();

    public NetworkServer(NetworkManager networkManager)
    {
        _networkManager = networkManager;
        _networkManager.ConnectionApprovalCallback += ApprovalCheck;

        _networkManager.OnServerStarted += OnNetworkReady;
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest req,
        NetworkManager.ConnectionApprovalResponse res)
    {
        UserData data = new UserData();
        data.Deserialize(req.Payload);

        _clientToAuthDictinary[req.ClientNetworkId] = data.userAuthId;
        _authToUserDataDictinary[data.userAuthId] = data;

        res.Approved = true;
        res.Position = TankSpawnPoint.GerRandomSpawnPos();
        res.Rotation = Quaternion.identity;
        //이나블 모든 위치중 랜덤 위치
        res.CreatePlayerObject = true;
    }

    private void OnNetworkReady()
    {
        _networkManager.OnClientDisconnectCallback += OnClientDisconnect;
    }

    private void OnClientDisconnect(ulong clientId)
    {
        if(_clientToAuthDictinary.TryGetValue(clientId, out string authID))
        {
            _clientToAuthDictinary.Remove(clientId);
            _authToUserDataDictinary.Remove(authID);
        }
    }

    public void Dispose()
    {
        if (_networkManager == null) return;

        _networkManager.ConnectionApprovalCallback -= ApprovalCheck;
        _networkManager.OnServerStarted -= OnNetworkReady;
        _networkManager.OnClientDisconnectCallback -= OnClientDisconnect;

        if(_networkManager.IsListening)
        {
            _networkManager.Shutdown();
        }
    }

    public UserData GetUserDataByClientId(ulong clientId)
    {
        if(_clientToAuthDictinary.TryGetValue(clientId, out string authId))
        {
            if(_authToUserDataDictinary.TryGetValue(authId, out UserData data))
            {
                return data;
            }
        }
        return null;
    }
}
