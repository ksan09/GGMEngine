using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{


    public string moveAxisName = "Horizontal";
    public string rotateAxisName = "Vertical";
    public string fireName = "Fire1";
    public string reloadName = "Reload";

    public Vector2 moveInput { get; private set; }
    public bool Fire => Input.GetButtonDown(fireName);
    public bool Reload => Input.GetButtonDown(reloadName);
   
    private Camera mainCam;
    public Action OnFirePressed;

    // Start is called before the first frame update
    void Awake()
    {
        mainCam = Camera.main;
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
            moveInput.Normalize();

        if(Fire)
        {
            OnFirePressed?.Invoke();
        }
    }

    public bool GetMouseWorldPosition(out Vector3 point)
    {
        Ray cameraRay = mainCam.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        float depth = mainCam.farClipPlane;

        point = Vector3.zero;

        if (Physics.Raycast(cameraRay, out hit, depth))
        {
            point = hit.point;
            return true;
        }

        return false;
    }
}
