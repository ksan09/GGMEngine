
using System;
using System.Text;
using Unity.Netcode;
using UnityEngine;

public class ClientGameManager : IDisposable
{



    public void ConnectClient(UserData userData)
    {
        NetworkManager.Singleton.NetworkConfig.ConnectionData = 
            Encoding.UTF8.GetBytes(JsonUtility.ToJson(userData));
        NetworkManager.Singleton.StartClient();

    }

    public void Dispose()
    {
        //�� �� ����
        var netMgr = NetworkManager.Singleton;

        if(netMgr != null && netMgr.IsConnectedClient)
        {
            netMgr.Shutdown();
        }
    }
}
