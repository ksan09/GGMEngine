using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ClientSingleton : MonoBehaviour
{
    public ClientGameManager GameManager { get; private set; }

    private static ClientSingleton _instance;
    public static ClientSingleton Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = FindObjectOfType<ClientSingleton>();
            if (_instance == null)
                Debug.LogError("No client singleton");

            return _instance;
        }
    }

    public void CreateClient()
    {
        GameManager = new ClientGameManager(NetworkManager.Singleton);
    }

    private void OnDestroy()
    {
        
    }

}
