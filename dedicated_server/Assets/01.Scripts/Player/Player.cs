using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using Cinemachine;

public class Player : NetworkBehaviour
{
    [SerializeField] private TextMeshPro _nameText;
    [SerializeField] private CinemachineVirtualCamera _playerCam;

    private NetworkVariable<FixedString32Bytes> _username = new();

    private void Awake()
    {
    }

    public override void OnNetworkSpawn()
    {

        if(IsOwner)
            _playerCam.Priority = 15;

        _username.OnValueChanged += HandleNameChanged;
        HandleNameChanged("", _username.Value);
    }

    public override void OnNetworkDespawn()
    {
        _username.OnValueChanged -= HandleNameChanged;
    }

    private void HandleNameChanged(FixedString32Bytes prev, FixedString32Bytes newValue)
    {
        // 이름이 바뀌었을 때 실행?
        _nameText.text = newValue.ToString();
    }

    public void SetUserName(string username)
    {
        _username.Value = username;
    }

}
