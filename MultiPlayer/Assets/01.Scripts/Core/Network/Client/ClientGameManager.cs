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
}
