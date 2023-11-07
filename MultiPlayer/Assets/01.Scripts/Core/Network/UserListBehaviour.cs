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

            // 여기에 ReadyUI에 호스트인지 일반 유저인지 보내주는 코드가 필요하다
            // 호스트는 스타트 버튼을 활성화하고 일반 유저는 스타트 버튼을 비활성화한다.

        }

        if(IsServer)
        {
            UserData userData = HostSingletone.Instance.GameManager.NetworkServer
                .GetUserDataByClientId(NetworkManager.Singleton.LocalClientId);

            HandleUserJoin(userData, NetworkManager.Singleton.LocalClientId);

            // 게임 시작버튼에 대한 구독
            _readyUI.GameStarted += HandleGameStarted;

            HostSingletone.Instance.GameManager.NetworkServer.OnClientJoin += HandleUserJoin;
            HostSingletone.Instance.GameManager.NetworkServer.OnClientLeft += HandleUserLeft;

            //레디 UI에다가 버튼들을 호스트에 맞게 만들어준다.
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
        if (idx >= 0) return; // 이미 존재하는 유저이다. 무시

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
        return -1; // 못 찾으면 -1
    }

    private void HandleUserLeft(UserData userData, ulong clientID)
    {
        if (_userList == null)
            return;

        // _userList 안에 있는 모든 유저를 뒤져서
        // clientID가 동일한 얘를 찾아 삭제후
        // 삭제된 게 UI에 반영되도록 HandleUserListChanged 작성
        foreach(var user in _userList)
        {
            if (user.clientID != clientID)
                continue;

            try
            {
                _userList.Remove(user);

            } catch(Exception e)
            {
                Debug.LogError($"{e.Message} - {clientID} 삭제중 오류 발생 : {user.playerName}");
            }
            break;
        }
    }

    private void HandleGameStarted()
    {
        
    }

    private void HandleTankSelected(int tankID)
    {
        // 내가 탱크를 변경한 것을 서버에게 알려주면 서버가 네트워크 리스트를 변경하고
        // 그 변경을 구독중이던 클라이언트들이 변경된다.
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
