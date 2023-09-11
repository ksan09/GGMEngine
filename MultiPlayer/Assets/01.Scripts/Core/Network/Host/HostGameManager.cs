
using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Lobbies;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using System.Collections;

public class HostGameManager
{
    private const string GameSceneName = "Game";
    private const int _maxConnections = 20;
    private string _joinCode;
    private string _lobbyId;
    private Allocation _allocation;

    private NetworkServer _networkServer;

    public async Task StartHostAsync()
    {
        try
        {
            _allocation = await Relay.Instance.CreateAllocationAsync(_maxConnections);
        } catch(Exception ex)
        {
            Debug.LogError(ex);
            return;
        }

        try
        {
            _joinCode = await Relay.Instance.GetJoinCodeAsync(_allocation.AllocationId);
            Debug.Log(_joinCode);
            //원래라면 이걸 UI에 띄워서 친구에게 불러줄 수 있어야 함
        } catch (Exception ex)
        {
            Debug.LogError(ex);
            return;
        }

        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

        var relayServerData = new RelayServerData(_allocation, "dtls");
        transport.SetRelayServerData(relayServerData);

        //로비 정보 받아오기
        try
        {
            CreateLobbyOptions option = new CreateLobbyOptions();
            option.IsPrivate = false; // 이걸 트루로 하면 join코드로만 참여가능한 방

            option.Data = new Dictionary<string, DataObject>()
            {
                {
                    "JoinCode",
                    new DataObject(visibility: DataObject.VisibilityOptions.Member,
                    value: _joinCode)
                }
            };

            Lobby lobby = await Lobbies.Instance
                .CreateLobbyAsync("Dummy lobby", _maxConnections, option);

            _lobbyId = lobby.Id;
            HostSingletone.Instance.StartCoroutine(HeartBeatLobby(15));

        } catch(LobbyServiceException ex)
        {
            Debug.LogError(ex); //원래라면 UI
            return;
        }

        _networkServer = new NetworkServer(NetworkManager.Singleton);
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager
            .LoadScene(GameSceneName, LoadSceneMode.Single);
    }

    IEnumerator HeartBeatLobby(int sec)
    {
        var timer = new WaitForSecondsRealtime(sec);
        while(true)
        {
            Lobbies.Instance.SendHeartbeatPingAsync(_lobbyId);
            yield return timer;
        }
    }
}
