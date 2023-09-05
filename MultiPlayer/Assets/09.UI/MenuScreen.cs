using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuScreen : MonoBehaviour
{
    private TextField _txtIpAddress;
    private TextField _txtPort;
    private TextField _txtJoincode;
    private UIDocument _uiDocument;
    private const string GameSceneName = "Game";

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback 
            += HandleClientDisconnected;

        var root = _uiDocument.rootVisualElement;
        _txtIpAddress = root.Q<TextField>("txt-ip-address");
        _txtPort = root.Q<TextField>("txt-port");
        _txtJoincode = root.Q<TextField>("txt-joincode");

        root.Q<Button>("btn-local-host")
            .RegisterCallback<ClickEvent>(OnHandleLocalHost);
        root.Q<Button>("btn-local-client")
            .RegisterCallback<ClickEvent>(OnHandleLocalClient);
        root.Q<Button>("btn-relay-host")
            .RegisterCallback<ClickEvent>(OnHandleRelayHost);
        root.Q<Button>("btn-joincode")
            .RegisterCallback<ClickEvent>(OnHandleRelayJoin);
    }

    private async void OnHandleRelayJoin(ClickEvent evt)
    {
        string code = _txtJoincode.value;
        await ClientSingletone.Instance.GameManager.StartClientAsync(code);
    }

    private async void OnHandleRelayHost(ClickEvent evt)
    {
        await HostSingletone.Instance.GameManager.StartHostAsync();
    }

    private void OnDisable()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback
            -= HandleClientDisconnected;
    }

    private void OnHandleLocalHost(ClickEvent evt)
    {
        //
        if (!SetUpNetworkPassport()) return;
        if (NetworkManager.Singleton.StartHost())
        {
            NetworkManager.Singleton.SceneManager
                .LoadScene(GameSceneName, LoadSceneMode.Single);
        }
        else
        {
            NetworkManager.Singleton.Shutdown();
        }
    }

    private void OnHandleLocalClient(ClickEvent evt)
    {
        //
        if (!SetUpNetworkPassport()) return;
        if (!NetworkManager.Singleton.StartClient())
        {
            NetworkManager.Singleton.Shutdown();
        }
    }

    private bool SetUpNetworkPassport()
    {
        var ip = _txtIpAddress.value;
        var port = _txtPort.value;

        var ipRegex = new Regex(@"^[0-9\.]+$");
        var portRegex = new Regex(@"^[0-9]{3,5}$");

        var ipMatch = ipRegex.Match(ip);
        var portMatch = portRegex.Match(port);

        if(!portMatch.Success || !ipMatch.Success)
        {
            Debug.LogError("올바르지 못한 아이피 또는 포트 번호입니다.");
            return false;
        }
        // 여기까지 왔다면 입력값은 전부 올바른거라고 봐야 해

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
            ip,
            (ushort)int.Parse(port)
            );

        return true;
    }

    private void HandleClientDisconnected(ulong obj)
    {
        //
        Debug.Log(obj + ", 에러발생");
    }


}
