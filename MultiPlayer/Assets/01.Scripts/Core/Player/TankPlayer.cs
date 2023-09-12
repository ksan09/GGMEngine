using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TankPlayer : NetworkBehaviour
{
    [Header("참조변수")]
    [SerializeField] private CinemachineVirtualCamera _followCam;

    [Header("셋팅값")]
    [SerializeField] private int _ownerCamPriority;

    public override void OnNetworkSpawn()
    {
        if(IsOwner)
        {
            _followCam.Priority = _ownerCamPriority;
        }
    }

}
