using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        HideCursor(true);
    }

    private void HideCursor(bool state)
    {
        Cursor.lockState = state ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !state;
    }

    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            HideCursor(false);
        }
        if(Input.GetMouseButtonDown(1))
        {
            HideCursor(true);
        }
    }
}
