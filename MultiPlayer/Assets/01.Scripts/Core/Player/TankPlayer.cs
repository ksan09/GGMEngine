using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class TankPlayer : NetworkBehaviour
{
    [Header("참조변수")]
    [SerializeField] private CinemachineVirtualCamera _followCam;
    [field: SerializeField] public Health HealthCompo { get; private set; }
    [field: SerializeField] public CoinCollector Coin { get; private set; }

    [Header("셋팅값")]
    [SerializeField] private int _ownerCamPriority;

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
