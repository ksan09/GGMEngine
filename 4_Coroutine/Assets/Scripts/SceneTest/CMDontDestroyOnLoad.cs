using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMDontDestroyOnLoad : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
