using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationController : MonoBehaviour
{
    [SerializeField] private ClientSingleton _clientPrefab;
    [SerializeField] private HostSingleton _hostPrefab;
    //���⿡ �ε��� ��巹���� ���� ����Ʈ
    [SerializeField] private NetworkObject _playerPrefab;

    public static event Action<string> OnMessageEvent;

    public static ApplicationController Instance;

    private void Awake()
    {
        Instance = this;    
    }

    private async void Start()
    {
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 60;

        OnMessageEvent?.Invoke("���� ���� �ʱ�ȭ ������...");
        await UnityServices.InitializeAsync();

        OnMessageEvent?.Invoke("��Ʈ��ũ ���� ������...");
        AuthenticationWrapper.OnMessageEvent += HandleAuthMessage;
        var state = await AuthenticationWrapper.DoAuth(3);

        if(state != AuthState.Authenticated) // ����Ƽ ���� ����
        {
            OnMessageEvent?.Invoke("�� ������ ���� �߻�.. ���� �ٽ� �����ϼ���.");
            return;
        }

        //ȣ��Ʈ ũ������Ʈ ����� �Ѵ�.
        HostSingleton host = Instantiate(_hostPrefab, transform);
        host.CreateHost(_playerPrefab);
        //Ŭ���̾�Ʈ ũ������Ʈ ����� �Ѵ�.
        ClientSingleton client = Instantiate(_clientPrefab, transform);
        client.CreateClient(); //���ӸŴ����� ������ش�.

        //���巹���� ���� �ε尡 �Ͼ�� �Ѵ�.


        //�޴������� �Ѿ��.
        SceneManager.LoadScene(SceneList.MenuScene);


    }

    private void HandleAuthMessage(string msg)
    {
        OnMessageEvent?.Invoke(msg);
    }

    private void OnDestroy()
    {
        AuthenticationWrapper.OnMessageEvent -= HandleAuthMessage;
    }
}
