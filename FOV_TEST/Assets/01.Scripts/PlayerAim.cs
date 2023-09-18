using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private LayerMask _whatIsGround;
    private Camera _mainCam;

    private void Start()
    {
        _mainCam = Camera.main;
    }

    private void LateUpdate()
    {
        Vector2 mousePos = _inputReader.AimPos;
        Ray ray = _mainCam.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, _mainCam.farClipPlane, _whatIsGround))
        {
            Vector3 worldMousePos = hitInfo.point;
            Vector3 dir = worldMousePos - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }
}