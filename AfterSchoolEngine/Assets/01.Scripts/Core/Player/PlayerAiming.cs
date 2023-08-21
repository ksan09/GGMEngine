using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAiming : NetworkBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Transform _turretTrm;

    private void LateUpdate()
    {
        if (IsOwner == false)
            return;

        Vector2 mousePos = _inputReader.AimPosition;
        Vector3 worldMousePos = CameraManager.Instance.MainCam.ScreenToWorldPoint(mousePos);
        Vector3 dir = (worldMousePos - transform.position).normalized;
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //_turretTrm.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        _turretTrm.up = new Vector2(dir.x, dir.y);
    }
}
