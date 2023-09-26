using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class TankPlayer : NetworkBehaviour
{
    [Header("��������")]
    [SerializeField] private CinemachineVirtualCamera _followCam;
    [field: SerializeField] public Health HealthCompo { get; private set; }
    [field: SerializeField] public CoinCollector Coin { get; private set; }

    [SerializeField] private SpriteRenderer _minimapIcon;

    [Header("���ð�")]
    [SerializeField] private int _ownerCamPriority;
    [SerializeField] private Color _ownerColor;

    public NetworkVariable<FixedString32Bytes> playerName = new NetworkVariable<FixedString32Bytes>();

    public static event Action<TankPlayer> OnPlayerSpawned;
    public static event Action<TankPlayer> OnPlayerDespawned;

    public override void OnNetworkSpawn()
    {
        if(IsServer) // �ִ� �ݟp������ ���� ����
        {
            //��Ʈ��ũ ���� ��ųʸ� �̿��ؼ� ��ũ�� �̸� �˾Ƴ���
            // �� �� �װ� �؟p������� �־��ٰ���
            var data = HostSingletone.Instance.GameManager.NetworkServer
                        .GetUserDataByClientId(OwnerClientId);
            playerName.Value = data.username;
            OnPlayerSpawned?.Invoke(this);
        }

        if(IsOwner)
        {
            _followCam.Priority = _ownerCamPriority;
            _minimapIcon.color = _ownerColor;

        }
    }

    public override void OnNetworkDespawn()
    {
        if(IsServer)
        {
            OnPlayerDespawned?.Invoke(this);
        }
    }
}
