using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientGameManager
{
    private const string MenuSceneName = "Menu";
    private JoinAllocation _allocation;

    public async Task<bool> InitAsync()
    {
        // �÷��̾� (����) ����
        await UnityServices.InitializeAsync(); //����Ƽ ���� �ʱ�ȭ

        //5�� �õ��ؼ� ���� ����� �޴´�.
        AuthState authState = await AuthenticationWrapper.DoAuth();

        if (authState == AuthState.Authenticated)
        {
            return true;
        }
        return false;
    }

    public void GotoMenu()
    {
        SceneManager.LoadScene(MenuSceneName);
    }

    public async Task StartClientAsync(string code)
    {
        //
        try
        {
            _allocation = await Relay.Instance.JoinAllocationAsync(code);

        } catch(Exception ex)
        {
            //������� UI�� ����ؾ���
            Debug.LogError(ex);
            return;
        }

        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

        var relayServerData = new RelayServerData(_allocation, "dtls");
        transport.SetRelayServerData(relayServerData);

        NetworkManager.Singleton.StartClient();
    }
}
