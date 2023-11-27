using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using Cinemachine;
using System;

public class Player : NetworkBehaviour
{
    [SerializeField] private TextMeshPro _nameText;
    [SerializeField] private CinemachineVirtualCamera _playerCam;

    public static event Action<Player> OnPlayerSpawned;
    public static event Action<Player> OnPlayerDespawned;

    public Health HealthCompo { get; private set; }

    private NetworkVariable<FixedString32Bytes> _username = new();

    private void Awake()
    {
        HealthCompo = GetComponent<Health>();
        HealthCompo.OnDie += HandleDie;
    }

    private void HandleDie(Health health)
    {
        Destroy(gameObject); // ����ٰ� ��ƼŬ�̳� �״� ȿ�� ���� �� ���;߰�����... �ϴ���.
    }

    public override void OnNetworkSpawn()
    {

        if(IsOwner)
            _playerCam.Priority = 15;

        if (IsServer)
        {
            OnPlayerSpawned?.Invoke(this);
        }

        _username.OnValueChanged += HandleNameChanged;
        HandleNameChanged("", _username.Value);
    }

    public override void OnNetworkDespawn()
    {
        _username.OnValueChanged -= HandleNameChanged;
        if(IsServer)
        {
            OnPlayerDespawned?.Invoke(this);
        }
    }

    private void HandleNameChanged(FixedString32Bytes prev, FixedString32Bytes newValue)
    {
        // �̸��� �ٲ���� �� ����?
        _nameText.text = newValue.ToString();
    }

    public void SetUserName(string username)
    {
        _username.Value = username;
    }

}
