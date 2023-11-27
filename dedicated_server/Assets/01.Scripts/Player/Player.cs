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
        Destroy(gameObject); // 여기다가 파티클이나 죽는 효과 같은 게 나와야겠지만... 일단은.
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
        // 이름이 바뀌었을 때 실행?
        _nameText.text = newValue.ToString();
    }

    public void SetUserName(string username)
    {
        _username.Value = username;
    }

}
