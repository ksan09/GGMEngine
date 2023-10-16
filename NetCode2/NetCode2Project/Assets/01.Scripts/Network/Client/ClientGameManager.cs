using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class ClientGameManager
{
    private NetworkManager _networkManager;
    private JoinAllocation _allocation;
    private bool _isLobbyRefresh = false; // 이건 나중에 씀

    public ClientGameManager(NetworkManager networkManager)
    {
        _networkManager = networkManager;


    }

    public void Disconnect()
    {
        //메뉴씬으로 보내기
        if(_networkManager.IsConnectedClient)
        {
            _networkManager.Shutdown(); //강제 종료
        }
    }

    public async Task StartClientAsync(string joinCode, UserData userData)
    {
        try
        {
            _allocation = await Relay.Instance.JoinAllocationAsync(joinCode);

        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return;
        }

        //트랜스포트
        //릴레이 서버 데이터 설정
        //userData -> json만들기 -> connectionData 넣기
        // NetworkManager에 startClient를 해주기
    }
}
