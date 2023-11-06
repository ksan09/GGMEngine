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
            // 능력 부족
            // 데디케이트 서버

        }
        else
        {
            // 우리는 여기만 일단 만들꺼다
            HostSingletone hostSingletone = Instantiate(_hostPrefab);
            hostSingletone.CreateHost(_playerPrefab); // 게임매니저 만들고 준비

            ClientSingletone clientSingletone = Instantiate(_clientPrefab);
            bool authenticated = await clientSingletone.CreateClient();

            if(authenticated)
            {
                ClientSingletone.Instance.GameManager.GotoMenu();
            }

            //여기까지 성공하면 메뉴씬으로 이동한다.

        }
    }
}
