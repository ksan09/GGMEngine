using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OffMeshClimb : MonoBehaviour
{
    [SerializeField]
    private int _offMeshArea = 3;
    [SerializeField]
    private float _climbSpeed = 1.5f;

    NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        StartCoroutine(StartClimb());
    }

    IEnumerator StartClimb()
    {
        while (true)
        {
            yield return new WaitUntil(() => IsOnClimb());
            yield return StartCoroutine(ClimbOrDescend());
        }
    }

    private bool IsOnClimb()
    {
        if (_agent.isOnOffMeshLink) //오프메시 링크에 와있는가?
        {
            //현재 위치의 오프 메시 데이터
            OffMeshLinkData linkData = _agent.currentOffMeshLinkData;

            //offMeshLink가 true이면 수동설정 OffMesh, false이면 자동설정 OffMesh
            if (linkData.offMeshLink != null && linkData.offMeshLink.area == _offMeshArea)
            {
                return true;
            }
        }

        return false;
    }

    private IEnumerator ClimbOrDescend()
    {
        _agent.isStopped = true;
        OffMeshLinkData linkData = _agent.currentOffMeshLinkData;
        Vector3 start = linkData.startPos;
        Vector3 end = linkData.endPos;

        //거속시 공식과 함께 보기

        float climbTime = Mathf.Abs(end.y - start.y) / _climbSpeed; //이거리를 오른다면 몇초?
        float currentTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / climbTime;

            transform.position = Vector3.Lerp(start, end, percent);

            yield return null;
        }
        _agent.CompleteOffMeshLink();
        _agent.isStopped = false;
    }
}
