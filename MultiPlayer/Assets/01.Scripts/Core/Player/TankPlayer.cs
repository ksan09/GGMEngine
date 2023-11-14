using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class TankPlayer : NetworkBehaviour
{
    [Header("��������")]
    [SerializeField] private SpriteRenderer _minimapIcon;
    [SerializeField] private CinemachineVirtualCamera _followCam;
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private ProjectileLauncher _launcher;
    [SerializeField] private SpriteRenderer _bodySprite;
    [SerializeField] private SpriteRenderer _turretSprite;

    [field: SerializeField] public Health HealthCompo { get; private set; }
    [field: SerializeField] public CoinCollector Coin { get; private set; }


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

    // �̰� ������ ȣ���ϴ� �޼����.
    public void SetTankNetworkVariable(UserListEntityState userState)
    {
        // ��ũ ���̵� ������� �ش� ��ũ�� ������ �ҷ����ְ�
        TankDataSO tankData = UserListBehaviour.Instance.GetTankDataSO(userState.tankID);
        // �̵����ٰ� �̵� ��������Ʈ��, ������Ʈ ����
        _movement.SetTankMovement(userState.combatData.moveSpeed, userState.combatData.rotateSpeed);
        // ���Ŀ��ٰ� ������ ����
        _launcher.SetDamage(tankData.basicTurretSO.damage);
        // ü�� ����
        HealthCompo.SetMaxHealth(tankData.maxHP);
    }

    [ClientRpc]
    public void SetTankVisualClientRPC(ulong clientID)
    {
        var user = UserListBehaviour.Instance.GetUserEntity(clientID);
        var tankData = UserListBehaviour.Instance.GetTankDataSO(user.tankID);

        // -- make --
        // body sprite
        _bodySprite.sprite = tankData.bodySprite;
        // turret sprite
        _turretSprite.sprite = tankData.basicTurretSprite;
        // turret luncher firepos
        _launcher.SetFirePosition(tankData.basicTurretSO.firePos);
    }
}
