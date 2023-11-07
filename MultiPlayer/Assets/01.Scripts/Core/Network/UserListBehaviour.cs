using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class UserListBehaviour : NetworkBehaviour
{
    [SerializeField] private ReadyUI _readyUI;
    [SerializeField] private List<TankDataSO> _tankDatas;

    public static UserListBehaviour Instance;
    public NetworkList<UserListEntityState> _userList = new NetworkList<UserListEntityState>();

    private void Awake()
    {
        Instance = this;
    }

    public override void OnNetworkSpawn()
    {
        _readyUI.SetTankTemplate(_tankDatas);
        _readyUI.TankSelect     += HandleTankSelected;
        _readyUI.ReadyChanged   += HandleReadyChanged;

        if(IsClient)
        {
            _userList.OnListChanged += HandleUserListChanged;
            foreach(var user in _userList)
            {
                HandleUserListChanged(new NetworkListEvent<UserListEntityState>
                {
                    Type = NetworkListEvent<UserListEntityState>.EventType.Add,
                    Value = user
                });
            }

            // ���⿡ ReadyUI�� ȣ��Ʈ���� �Ϲ� �������� �����ִ� �ڵ尡 �ʿ��ϴ�
            // ȣ��Ʈ�� ��ŸƮ ��ư�� Ȱ��ȭ�ϰ� �Ϲ� ������ ��ŸƮ ��ư�� ��Ȱ��ȭ�Ѵ�.

        }

        if(IsServer)
        {
            UserData userData = HostSingletone.Instance.GameManager.NetworkServer
                .GetUserDataByClientId(NetworkManager.Singleton.LocalClientId);

            HandleUserJoin(userData, NetworkManager.Singleton.LocalClientId);

            // ���� ���۹�ư�� ���� ����
            _readyUI.GameStarted += HandleGameStarted;

            HostSingletone.Instance.GameManager.NetworkServer.OnClientJoin += HandleUserJoin;
            HostSingletone.Instance.GameManager.NetworkServer.OnClientLeft += HandleUserLeft;

            //���� UI���ٰ� ��ư���� ȣ��Ʈ�� �°� ������ش�.
        }

    }

    public override void OnNetworkDespawn()
    {
        _readyUI.TankSelect -= HandleTankSelected;
        _readyUI.ReadyChanged -= HandleReadyChanged;

        if(IsClient)
        {
            _userList.OnListChanged -= HandleUserListChanged;
        }

        if(IsServer)
        {
            _readyUI.GameStarted -= HandleGameStarted;

            HostSingletone.Instance.GameManager.NetworkServer.OnClientJoin -= HandleUserJoin;
            HostSingletone.Instance.GameManager.NetworkServer.OnClientLeft -= HandleUserLeft;
        }
        
    }

    private void HandleUserJoin(UserData userData, ulong clientID)
    {
        int idx = FindIndex(clientID);
        if (idx >= 0) return; // �̹� �����ϴ� �����̴�. ����

        UserListEntityState newUser = new UserListEntityState
        {
            clientID = clientID,
            playerName = userData.username,
            ready = false,
            tankID = 0
        };
        _userList.Add(newUser);
    }

    private int FindIndex(ulong clientID)
    {
        for(int i = 0; i < _userList.Count; ++i)
        {
            if (_userList[i].clientID != clientID) continue;
            return i;
        }
        return -1; // �� ã���� -1
    }

    private void HandleUserLeft(UserData userData, ulong clientID)
    {
        if (_userList == null)
            return;

        // _userList �ȿ� �ִ� ��� ������ ������
        // clientID�� ������ �긦 ã�� ������
        // ������ �� UI�� �ݿ��ǵ��� HandleUserListChanged �ۼ�
        foreach(var user in _userList)
        {
            if (user.clientID != clientID)
                continue;

            try
            {
                _userList.Remove(user);

            } catch(Exception e)
            {
                Debug.LogError($"{e.Message} - {clientID} ������ ���� �߻� : {user.playerName}");
            }
            break;
        }
    }

    private void HandleGameStarted()
    {
        
    }

    private void HandleTankSelected(int tankID)
    {
        // ���� ��ũ�� ������ ���� �������� �˷��ָ� ������ ��Ʈ��ũ ����Ʈ�� �����ϰ�
        // �� ������ �������̴� Ŭ���̾�Ʈ���� ����ȴ�.
        SelectTankServerRpc(tankID, NetworkManager.Singleton.LocalClientId);
    }

    private void HandleReadyChanged(bool value)
    {
        ReadyTankServerRpc(value, NetworkManager.Singleton.LocalClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SelectTankServerRpc(int tankID, ulong clientID)
    {
        int idx = FindIndex(clientID);

        UserListEntityState oldUser = _userList[idx];

        _userList[idx] = new UserListEntityState
        {
            tankID = tankID,
            clientID = oldUser.clientID,
            playerName = oldUser.playerName,
            combatData = oldUser.combatData,
            ready = oldUser.ready
        };
    }

    [ServerRpc(RequireOwnership = false)]
    private void ReadyTankServerRpc(bool ready, ulong clientID)
    {
        int idx = FindIndex(clientID);

        UserListEntityState oldUser = _userList[idx];

        _userList[idx] = new UserListEntityState
        {
            tankID =oldUser.tankID,
            clientID = oldUser.clientID,
            playerName = oldUser.playerName,
            combatData = oldUser.combatData,
            ready = ready
        };
    }

    private void HandleUserListChanged(NetworkListEvent<UserListEntityState> evt)
    {
        switch (evt.Type)
        {
            case NetworkListEvent<UserListEntityState>.EventType.Add:
                _readyUI.AddUserData(evt.Value);
                break;

            case NetworkListEvent<UserListEntityState>.EventType.Remove:
                _readyUI.RemoveUserData(evt.Value);
                break;

            case NetworkListEvent<UserListEntityState>.EventType.Value:
                _readyUI.UpdateUserData(evt.Value);
                break;
        }
    }


}
