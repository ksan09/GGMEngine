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
    [Header("참조변수")]
    [SerializeField] private SpriteRenderer _minimapIcon;
    [SerializeField] private CinemachineVirtualCamera _followCam;
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private ProjectileLauncher _launcher;
    [SerializeField] private SpriteRenderer _bodySprite;
    [SerializeField] private SpriteRenderer _turretSprite;

    [field: SerializeField] public Health HealthCompo { get; private set; }
    [field: SerializeField] public CoinCollector Coin { get; private set; }


    [Header("셋팅값")]
    [SerializeField] private int _ownerCamPriority;
    [SerializeField] private Color _ownerColor;

    public NetworkVariable<FixedString32Bytes> playerName = new NetworkVariable<FixedString32Bytes>();

    public static event Action<TankPlayer> OnPlayerSpawned;
    public static event Action<TankPlayer> OnPlayerDespawned;

    public override void OnNetworkSpawn()
    {
        if(IsServer) // 애는 넷웤서버가 있을 예정
        {
            //네트워크 서버 딕셔너리 이용해서 탱크의 이름 알아내기
            // 그 후 그걸 넥웤베리어블에 넣어줄거임
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

    // 이건 서버만 호출하는 메서드야.
    public void SetTankNetworkVariable(UserListEntityState userState)
    {
        // 탱크 아이디를 기반으로 해당 탱크의 정보를 불러와주고
        TankDataSO tankData = UserListBehaviour.Instance.GetTankDataSO(userState.tankID);
        // 이동에다가 이동 스프라이트와, 로테이트 설정
        _movement.SetTankMovement(userState.combatData.moveSpeed, userState.combatData.rotateSpeed);
        // 런쳐에다가 데미지 설정
        _launcher.SetDamage(tankData.basicTurretSO.damage);
        // 체력 설정
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
