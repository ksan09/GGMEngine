
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
            //������� �̰� UI�� ����� ģ������ �ҷ��� �� �־�� ��
        } catch (Exception ex)
        {
            Debug.LogError(ex);
            return;
        }

        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

        var relayServerData = new RelayServerData(_allocation, "dtls");
        transport.SetRelayServerData(relayServerData);

        //�κ� ���� �޾ƿ���
        try
        {
            CreateLobbyOptions option = new CreateLobbyOptions();
            option.IsPrivate = false; // �̰� Ʈ��� �ϸ� join�ڵ�θ� ���������� ��

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
            Debug.LogError(ex); //������� UI
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
