using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAiming : NetworkBehaviour
{
    // 
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Transform _handTrm;

    private Camera _mainCam;

    private void Start()
    {
        _mainCam = Camera.main;
    }

    // 여기선 마우스 위치를 _inputReader AimPosition을 받아서
    // 적절하게 handTrm을 회전시켜주면 된다.
    private void LateUpdate()
    {
        if (!IsOwner) return;

        Vector2 worldPos = _mainCam.ScreenToWorldPoint(_inputReader.AimPosition);
        Vector2 dir = ((Vector3)worldPos - transform.position).normalized;

        _handTrm.right = dir;
    }



}
