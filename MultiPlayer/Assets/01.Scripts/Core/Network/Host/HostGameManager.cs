
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
using Unity.Services.Authentication;

public class HostGameManager : IDisposable
{
    private const string GameSceneName = "Game";
    private const int _maxConnections = 20;
    private string _joinCode;
    private string _lobbyId;
    private Allocation _allocation;

    public NetworkServer NetworkServer { get; private set; }

    private NetworkObject _playerPrefab;

    public HostGameManager(NetworkObject playerPrefab)
    {
        _playerPrefab = playerPrefab;
    }

    public async void ShutdownAsync()
    {
        HostSingletone.Instance.StopCoroutine(nameof(HeartBeatLobby));
        if(!string.IsNullOrEmpty(_lobbyId))
        {
            try
            {
                await Lobbies.Instance.DeleteLobbyAsync(_lobbyId);
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError(e);
            }
        }

        NetworkServer.OnClientLeft -= HandleClientLeft;
        _lobbyId = String.Empty;
        NetworkServer?.Dispose();
    }

    public void Dispose()
    {
        ShutdownAsync();
    }

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

        string playerName = PlayerPrefs.GetString(BootstrapScreen.PlayerNameKey, "Unknown");

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
                .CreateLobbyAsync($"{playerName}'s lobby", _maxConnections, option);

            _lobbyId = lobby.Id;
            HostSingletone.Instance.StartCoroutine(HeartBeatLobby(15));

        } catch(LobbyServiceException ex)
        {
            Debug.LogError(ex); //원래라면 UI
            return;
        }

        NetworkServer = new NetworkServer(NetworkManager.Singleton, _playerPrefab);

        UserData userData = new UserData
        {
            username = playerName,
            userAuthId = AuthenticationService.Instance.PlayerId
        };
        NetworkManager.Singleton.NetworkConfig.ConnectionData = userData.Serialize().ToArray();

        NetworkManager.Singleton.StartHost();
        NetworkServer.OnClientLeft += HandleClientLeft;
        NetworkManager.Singleton.SceneManager
            .LoadScene(GameSceneName, LoadSceneMode.Single);
    }

    private async void HandleClientLeft(UserData userData, ulong clientID)
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(_lobbyId, userData.userAuthId);
        } catch(LobbyServiceException ex)
        {
            Debug.LogError(ex);
        }
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
