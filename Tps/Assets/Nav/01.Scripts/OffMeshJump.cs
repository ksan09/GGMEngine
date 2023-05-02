using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OffMeshJump : MonoBehaviour
{
    [SerializeField]
    private float _jumpSpeed = 10.0f;
    [SerializeField]
    private float _gravity = -9.81f;
    private NavMeshAgent _navAgent;

    private void Awake()
    {
        _navAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        StartCoroutine(StartJumpCo());
    }

    IEnumerator StartJumpCo()
    {
        while (true)
        {
            yield return new WaitUntil(() => IsOnJump());

            yield return StartCoroutine(JumpTo());
        }
    }

    public bool IsOnJump()
    {

        if (_navAgent.isOnOffMeshLink) //�����޽ÿ� �ְ� ���� �����Ͷ��
        {
            OffMeshLinkData linkData = _navAgent.currentOffMeshLinkData;

            if (linkData.linkType == OffMeshLinkType.LinkTypeJumpAcross ||
                linkData.linkType == OffMeshLinkType.LinkTypeDropDown)
            {
                return true;
            }
        }
        return false;
    }




    IEnumerator JumpTo()
    {
        _navAgent.isStopped = true;

        OffMeshLinkData linkData = _navAgent.currentOffMeshLinkData;
        Vector3 start = transform.position;
        Vector3 end = linkData.endPos;

        float jumpTime = Mathf.Max(0.3f, Vector3.Distance(start, end) / _jumpSpeed);
        float currentTime = 0;
        float percent = 0;

        float v0 = (end - start).y - _gravity; // y���� �ʱ� �ӵ� 

        while (percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / jumpTime;

            //�ð� ����� ���� ��ġ�� �ٲ��ش�
            Vector3 pos = Vector3.Lerp(start, end, percent);

            //������ � : ������ġ + �ʱ�ӵ� * �ð� + �߷� * �ð�����
            pos.y = start.y + (v0 * percent) + (_gravity * percent * percent);
            transform.position = pos;

            yield return null;
        }

        _navAgent.CompleteOffMeshLink();

        _navAgent.isStopped = false;
    }
}