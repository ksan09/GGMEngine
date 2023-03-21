using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput1 : MonoBehaviour
{


    public string moveAxisName = "Horizontal";
    public string rotateAxisName = "Vertical";
    public string fireName = "Fire1";
    public string reloadName = "Reload";

    public Vector2 moveInput { get; private set; }
    public bool Fire => Input.GetButtonDown(fireName);
    public bool Reload => Input.GetButtonDown(reloadName);

    public LayerMask whatIsGround; // 땅바닥 인지 마스크 레이어
    public Vector3 mousePos { get; private set; } // 마우스 포인터 위치 벡터


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InputKey();
    }

    private void InputKey()
    {
        float x = Input.GetAxisRaw(moveAxisName);
        float y = Input.GetAxisRaw(rotateAxisName);

        moveInput = new Vector2(x, y);

        if(moveInput.sqrMagnitude > 1)
        {
            moveInput.Normalize();
        }

        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        float depth = Camera.main.farClipPlane;

        if(Physics.Raycast(cameraRay, out hit, depth, whatIsGround))
        {
            mousePos = hit.point;
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(mousePos, 0.5f);
    }
}
