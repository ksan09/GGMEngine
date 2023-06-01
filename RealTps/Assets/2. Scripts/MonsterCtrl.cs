using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class MonsterCtrl : PoolableMono
{
    public UnityEvent OnDamageCast;

    public enum State
    {
        IDLE,
        TRACE,
        ATTACK,
        DIE
    }

    public State state = State.IDLE;


    public float traceDist = 10.0f; //추적 사정거리
    public float attackDist = 2.0f;  //공격 사정거리
    public bool isDie = false; //몬스터 사망 여부

    private Transform playerTr;
    private NavMeshAgent agent;
    private Animator anim;

    //Animator 파라미터의 해시값 추출
    private readonly int hashTrace = Animator.StringToHash("IsTrace");
    private readonly int hashAttack = Animator.StringToHash("IsAttack");
    private readonly int hashDie= Animator.StringToHash("Death");

    private void Awake()
    {
        playerTr = GameManager.instance.PlayerTr; //플레이어 불러오고
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        //NavMeshAgent의 자동 회전 기능 비활성화
        agent.updateRotation = false;
    }
    
   


    private void Update()
    {
        if(agent.remainingDistance >= 2.0f)
        {
            //목적지로 향햐는 목표 속도. 하지만 관성이나 장애물에 의해 실제 속도와는 차이가 날 수 있음.
            //장애물 회피를 고려한 이동 방향
            Vector3 direction = agent.desiredVelocity;
            Quaternion rot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 10.0f);
        }
    }

    IEnumerator checkMonsterState()
    {
        while (!isDie)
        {
            if (state == State.DIE)
            {
                yield break;
            }

            //  float distance = Vector3.Distance(playerTr.position, transform.position);
            float dist = (playerTr.position - transform.position).sqrMagnitude;

            if (dist <= attackDist * attackDist)
            {
                state = State.ATTACK;
            }
            else if (dist <= traceDist * traceDist)
            {
                state = State.TRACE;
            }
            else if ((state == State.TRACE || state == State.ATTACK) && dist <= traceDist * traceDist)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.IDLE;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    //몬스터의 상태에 따라 몬스터의 동작 수행
    IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            switch (state)
            {
                case State.IDLE:
                    agent.isStopped = true;
                    anim.SetBool(hashTrace, false);
                    break;
                case State.TRACE:
                    agent.SetDestination(playerTr.position);
                    agent.isStopped = false;
                    anim.SetBool(hashTrace, true);
                    anim.SetBool(hashAttack, false);
                    break;
                case State.ATTACK:
                    anim.SetBool(hashAttack, true);
                    break;
                case State.DIE:
                    isDie = true;
                    agent.isStopped = true;
                    anim.SetTrigger(hashDie);
                    // GetComponent<CapsuleCollider>().enabled = false;
                    yield return new WaitForSeconds(1f);
                    PoolManager.Instance.Push(this);
                    break;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    private bool checkPlayer()
    {
        Vector3 bias = transform.forward;
        Vector3 pos = transform.position;
        pos.y += 1; //ray를 player의 몸통 높이까지 올리기 위해
        for (int i = -60; i <= 60; i += 10)
        {
            Vector3 dir = Quaternion.Euler(0, -i, 0) * bias;
            Ray ray = new Ray(pos, dir.normalized);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, traceDist))
            {
                if (hit.collider.tag == "Player")
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        //추적 거리 표시
        if (state == State.TRACE)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, traceDist);
        }

        //공격 거리 표시
        if (state == State.ATTACK)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackDist);
        }

        Vector3 bias = transform.forward;
        Vector3 pos = transform.position;
        pos.y += 1;
        Gizmos.color = Color.red;
        for (int i = -60; i <= 60; i += 10)
        {
            Vector3 dir = Quaternion.Euler(0, -i, 0) * bias;
            Gizmos.DrawRay(pos, dir * traceDist);
        }
    }


    public void OnAnimationHit()
    {
        OnDamageCast?.Invoke();
    }

    public override void Init()
    {
        LivingEntity livingEntity = this.GetComponent<LivingEntity>();
        livingEntity.initHealth = 100;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnEnable()
    {
        isDie = false;
        state = State.IDLE;
        StartCoroutine(checkMonsterState());
        StartCoroutine(MonsterAction());
    }
}


