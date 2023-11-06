using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class ApplicationControleer : MonoBehaviour
{
    [SerializeField]
    private ClientSingletone _clientPrefab;
    [SerializeField]
    private HostSingletone _hostPrefab;

    [SerializeField]
    private NetworkObject _playerPrefab;

    private async void Start()
    {
        DontDestroyOnLoad(gameObject);

        await LaunchInMode(SystemInfo.graphicsDeviceType == 
            UnityEngine.Rendering.GraphicsDeviceType.Null);
    }

    private async Task LaunchInMode(bool isDedicatedServer)
    {
        if(isDedicatedServer)
        {
            // �ɷ� ����
            // ��������Ʈ ����

        }
        else
        {
            // �츮�� ���⸸ �ϴ� ���鲨��
            HostSingletone hostSingletone = Instantiate(_hostPrefab);
            hostSingletone.CreateHost(_playerPrefab); // ���ӸŴ��� ����� �غ�

            ClientSingletone clientSingletone = Instantiate(_clientPrefab);
            bool authenticated = await clientSingletone.CreateClient();

            if(authenticated)
            {
                ClientSingletone.Instance.GameManager.GotoMenu();
            }

            //������� �����ϸ� �޴������� �̵��Ѵ�.

        }
    }
}
