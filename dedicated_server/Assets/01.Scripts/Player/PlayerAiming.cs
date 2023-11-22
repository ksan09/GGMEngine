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

    // ���⼱ ���콺 ��ġ�� _inputReader AimPosition�� �޾Ƽ�
    // �����ϰ� handTrm�� ȸ�������ָ� �ȴ�.
    private void LateUpdate()
    {
        if (!IsOwner) return;

        Vector2 worldPos = _mainCam.ScreenToWorldPoint(_inputReader.AimPosition);
        Vector2 dir = ((Vector3)worldPos - transform.position).normalized;

        _handTrm.right = dir;
    }



}
