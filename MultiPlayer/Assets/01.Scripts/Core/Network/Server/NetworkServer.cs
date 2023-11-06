using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkServer : IDisposable
{
    private NetworkManager _networkManager;
    public Action<string> OnClientLeft;

    private Dictionary<ulong, string> _clientToAuthDictinary = new Dictionary<ulong, string>();
    private Dictionary<string, UserData> _authToUserDataDictinary = new Dictionary<string, UserData>();

    private NetworkObject _playerPrefab;

    public NetworkServer(NetworkManager networkManager, NetworkObject playerPrefab)
    {
        _networkManager = networkManager;
        _networkManager.ConnectionApprovalCallback += ApprovalCheck;

        _networkManager.OnServerStarted += OnNetworkReady;
        _playerPrefab = playerPrefab;
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest req,
        NetworkManager.ConnectionApprovalResponse res)
    {
        UserData data = new UserData();
        data.Deserialize(req.Payload);

        _clientToAuthDictinary[req.ClientNetworkId] = data.userAuthId;
        _authToUserDataDictinary[data.userAuthId] = data;

        res.Approved = true;
        
        //res.Position = TankSpawnPoint.GerRandomSpawnPos();
        //res.Rotation = Quaternion.identity;
        ////�̳��� ��� ��ġ�� ���� ��ġ
        //res.CreatePlayerObject = true;
    }

    //�̳༮�� ����� ���� ������ ��� �Ѵ�.
    public void SpawnPlayer(ulong clientID, UserListEntityState userState)
    {
        NetworkObject player = GameObject.Instantiate(_playerPrefab, 
            TankSpawnPoint.GerRandomSpawnPos(), Quaternion.identity);

        //�ش� �÷��̾� ������Ʈ clientID���� ���ʷ� �Ҵ�
        player.SpawnAsPlayerObject(clientID);

        TankPlayer tankComponent = player.GetComponent<TankPlayer>();

        tankComponent.SetTankNetworkVariable(userState);
        //�ش� �÷��̾ ������ ������ �����Ų��.


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

            OnClientLeft?.Invoke(authID);
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
