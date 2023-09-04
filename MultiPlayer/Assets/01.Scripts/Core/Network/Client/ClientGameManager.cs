using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientGameManager
{
    private const string MenuSceneName = "Menu";

    public async Task<bool> InitAsync()
    {
        // 플레이어 (게임) 인증
        await UnityServices.InitializeAsync(); //유니티 서비스 초기화

        //5번 시도해서 나온 결과를 받는다.
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
}
