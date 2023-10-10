using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Core;
using UnityEngine;

public class ApplicationController : MonoBehaviour
{
    [SerializeField] private ClientSingleton _clientPrefab;
    [SerializeField] private HostSingleton _hostPrefab;
    //여기에 로딩할 어드레서블 에셋 리스트
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

        OnMessageEvent?.Invoke("게임 서비스 초기화 진행중...");
        await UnityServices.InitializeAsync();

        OnMessageEvent?.Invoke("네트워크 서비스 인증중...");
        AuthenticationWrapper.OnMessageEvent += HandleAuthMessage;
        var state = await AuthenticationWrapper.DoAuth(3);

        if(state != AuthState.Authenticated) // 유니티 인증 실패
        {
            OnMessageEvent?.Invoke("앱 인증중 오류 발생.. 앱을 다시 시작하세요.");
            return;
        }

        HostSingleton host = Instantiate(_hostPrefab, transform);
        //호스트 크리에이트 해줘야 한다.
        host.CreateHost(_playerPrefab);
        ClientSingleton client = Instantiate(_clientPrefab, transform);
        //클라이언트 크리에이트 해줘야 한다.


        //에드레서블 에셋 로드가 일어나야 한다.


        //메뉴씬으로 넘어간다.
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
