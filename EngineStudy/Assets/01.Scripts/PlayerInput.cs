using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string moveAxisName = "Horizontal";
    public string rotateAxisName = "Vertical";

    public Vector2 moveInput { get; private set; }
   

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
    }
}
