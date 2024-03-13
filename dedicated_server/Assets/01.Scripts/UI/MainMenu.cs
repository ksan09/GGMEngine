using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField _txtIP;
    [SerializeField] private TMP_InputField _txtPort;
    [SerializeField] private TMP_InputField _txtUsername;

    public void ConnectedToServer()
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
            _txtIP.text,
            (ushort)int.Parse(_txtPort.text)
        );

        string name = _txtUsername.text;
        UserData userData = new UserData
        {
            username = name
        };

        ClientSingleton.Instance.StartClient(userData);
    }
}
