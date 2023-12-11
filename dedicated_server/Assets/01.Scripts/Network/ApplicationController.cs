using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationController : MonoBehaviour
{
    [SerializeField] private NetworkObject _playerPrefab;
    [SerializeField] private ServerSingleton _serverPrefab;
    [SerializeField] private ClientSingleton _clientPrefab;

    [SerializeField] private string _ipAddress;
    [SerializeField] private ushort _port;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        LaunchByMode(SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null);
    }

    private string GetLocalIP()
    {
        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

        foreach (IPAddress address in host.AddressList)
        {
            //���ͳ�Ʈ��ũ IP�� ��츸
            if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return address.ToString();
            }
        }
        return string.Empty;
    }

    private void LaunchByMode(bool isDedicatedServer)
    {
        if (isDedicatedServer)
        {
            string ipAddress = _ipAddress;
            if(!string.IsNullOrEmpty(ipAddress))
            {
                _ipAddress = ipAddress;
            }

            //Server
            ServerSingleton server = Instantiate(_serverPrefab, transform);
            server.StartServer(_playerPrefab, _ipAddress, _port);
        }
        else
        {
            //Client
            ClientSingleton client = Instantiate(_clientPrefab, transform);
            client.CreateClient(_ipAddress, _port);

            SceneManager.LoadScene(SceneList.Menu);
        }
    }
}
