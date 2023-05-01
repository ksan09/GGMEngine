using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavPlayerMovement : MonoBehaviour
{
    private NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        MouseClick();
    }

    private void MouseClick()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 pos;

            if(NavGameManager.Instance.GetMouseWorldPosition(out pos))
            {
                _agent.SetDestination(pos);
            }
        }
    }
}
