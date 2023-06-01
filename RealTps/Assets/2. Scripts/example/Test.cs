using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public BaseMonster[] monsters;

    private void Start()
    {
        for(int i =0; i < monsters.Length; i++)
        {
            Debug.Log(monsters[i].gameObject.name);
        }
    }
}
