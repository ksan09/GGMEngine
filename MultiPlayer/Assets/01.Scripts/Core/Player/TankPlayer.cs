using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TankPlayer : NetworkBehaviour
{
    [Header("��������")]
    [SerializeField] private CinemachineVirtualCamera _followCam;

    [Header("���ð�")]
    [SerializeField] private int _ownerCamPriority;

    public override void OnNetworkSpawn()
    {
        if(IsOwner)
        {
            _followCam.Priority = _ownerCamPriority;
        }
    }

}
