using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class MonsterControl : PoolableMono
{
    public enum State
    {
        IDLE,
        TRACE,
        ATTACK,
        DIE
    }

    public State state = State.IDLE;

    public float traceDist = 10.0f;
    public float attackDist = 2.0f;
    public bool isDie = false;

    public bool isTrace = false;
    public bool isAttack = false;

    private Transform playerTr;
    private NavMeshAgent agent;
    private Animator playerAnimator;

    private readonly int hashTrace = Animator.StringToHash("isTrace");
    private readonly int hashAttack = Animator.StringToHash("isAttack");

    public UnityEvent OnDamageCast;

    private void Awake()
    {
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        playerAnimator = GetComponent<Animator>();
        state = State.TRACE;

        StartCoroutine(CheckMonsterState());
        StartCoroutine(MonsterAction());
    }


    private bool CheckPlayer()
    {
        Vector3 bias = transform.forward;
        Vector3 pos = transform.position;

        pos.y += 1.8f; // 몸통 높이로 올리기

        for(int i = -60; i <= 60; i+=10)
        {
            Vector3 dir = Quaternion.Euler(0, i, 0) * bias;

            Ray ray = new Ray(pos, dir.normalized);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, traceDist))
            {
                if ( hit.collider.tag == "Player")
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void OnAnimationHit()
    {
        OnDamageCast?.Invoke();
    }

    private void OnDrawGizmos()
    {
        Vector3 bias = transform.forward;
        Vector3 pos = transform.position;
        pos.y += 1.8f;
        Gizmos.color = Color.red;
        for(int i = -60; i <= 60; ++i)
        {
            Vector3 dir = Quaternion.Euler(0, i, 0) * bias;
            Gizmos.DrawRay(pos, dir * traceDist);
        }
    }

    IEnumerator CheckMonsterState()
    {
        while(!isDie)
        {
            if(state == State.DIE)
            {
                yield break;
            }

            float dist = (playerTr.position - transform.position).sqrMagnitude;

            if (dist <= attackDist * attackDist)
                state = State.ATTACK;
            else if (dist <= traceDist * traceDist)
                state = State.TRACE;
            else
                state = State.IDLE;

            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator MonsterAction()
    {
        while(!isDie)
        {
            switch (state)
            {
                case State.IDLE:
                    {
                        agent.isStopped = true;
                        isAttack = false;
                        isTrace = false;
                    }
                    break;
                case State.TRACE:
                    {
                        agent.isStopped = false;
                        agent.destination = playerTr.position;
                        isAttack = false;
                        isTrace = true;
                    }
                    break;
                case State.ATTACK:
                    {
                        isAttack = true;
                        isTrace = false;
                    }
                    break;
                case State.DIE:
                    agent.isStopped = true;
                    playerAnimator.SetTrigger("Death");
                    isDie = true;
                    GetComponent<CapsuleCollider>().enabled = false;
                    StartCoroutine("Delay");
                    break;
                default:
                    break;
            }

            playerAnimator.SetBool(hashAttack, isAttack);
            playerAnimator.SetBool(hashTrace, isTrace);

            yield return new WaitForSeconds(0.3f);
        }
    }

    public override void Init()
    {
        isDie = false;
        GetComponent<CapsuleCollider>().enabled = true;
    }

    private void OnEnable()
    {
        StartCoroutine(CheckMonsterState());
        StartCoroutine(MonsterAction());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        PoolManager.Instance.Push(this);
    }
}
