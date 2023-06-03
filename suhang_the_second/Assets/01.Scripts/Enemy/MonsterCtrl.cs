using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class MonsterCtrl : MonoBehaviour
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

    private bool isAttack = false;
    private float currentTime = 0f;
    private float attackCoolTime = 1f;

    //Animator 파라미터의 해시값 추출
    private readonly int hashMove = Animator.StringToHash("is_move");
    private readonly int hashMoveTrigger = Animator.StringToHash("move");
    private readonly int hashAttack = Animator.StringToHash("is_attack");
    private readonly int hashAttackTrigger = Animator.StringToHash("attack");
    private readonly int hashDie= Animator.StringToHash("is_dead");
    private readonly int hashDieTrigger = Animator.StringToHash("dead");

    private void Awake()
    {
        playerTr = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        //NavMeshAgent의 자동 회전 기능 비활성화
        agent.updateRotation = false;

        currentTime = attackCoolTime;
        isDie = false;
        state = State.IDLE;
        StartCoroutine(checkMonsterState());
        StartCoroutine(MonsterAction());
    }
    
   


    private void Update()
    {
        currentTime += Time.deltaTime;
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

            if ((dist <= attackDist * attackDist && currentTime >= attackCoolTime) || isAttack)
            {
                state = State.ATTACK;
            }
            else if (dist <= traceDist * traceDist && checkPlayer())
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
                    anim.ResetTrigger(hashAttackTrigger);
                    anim.ResetTrigger(hashMoveTrigger);
                    anim.ResetTrigger(hashDieTrigger);
                    anim.SetBool(hashMove, false);
                    anim.SetBool(hashAttack, false);
                    break;
                case State.TRACE:
                    agent.SetDestination(playerTr.position);
                    agent.isStopped = false;
                    anim.ResetTrigger(hashAttackTrigger);
                    anim.SetTrigger(hashMoveTrigger);
                    anim.ResetTrigger(hashDieTrigger);
                    anim.SetBool(hashMove, true);
                    anim.SetBool(hashAttack, false);
                    break;
                case State.ATTACK:
                    isAttack = true;
                    anim.SetTrigger(hashAttackTrigger);
                    anim.ResetTrigger(hashMoveTrigger);
                    anim.ResetTrigger(hashDieTrigger);
                    anim.SetBool(hashMove, false);
                    anim.SetBool(hashAttack, true);
                    break;
                case State.DIE:
                    anim.ResetTrigger(hashAttackTrigger);
                    anim.ResetTrigger(hashMoveTrigger);
                    anim.SetTrigger(hashDieTrigger);
                    isDie = true;
                    agent.isStopped = true;
                    anim.SetBool(hashDie, true);
                    GetComponent<CapsuleCollider>().enabled = false;
                    yield return new WaitForSeconds(1f);
                    //PoolManager.Instance.Push(this);
                    break;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    private bool checkPlayer()
    {
        Vector3 dir = playerTr.position - transform.position;
        Ray ray = new Ray(transform.position + Vector3.up, dir.normalized);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, traceDist))
        {
            if (hit.collider.tag == "Player")
            {
                return true;
            }
        }
        return false;
    }

    public void OnAttackAnimation()
    {
        OnDamageCast?.Invoke();
    }

    public void OnAnimationEnd() // 애니메이션 이벤트에 넣을 예정
    {
        anim.ResetTrigger(hashAttackTrigger);
        isAttack = false;
        currentTime = 0;
    }

    public void Init()
    {
        //LivingEntity livingEntity = this.GetComponent<LivingEntity>();
        //livingEntity.initHealth = 100;
    }
    public void Dead()
    {
        state = State.DIE;
        GameManager.Instance.KillMonster();
    }

    private void OnEnable()
    {
        isDie = false;
        state = State.IDLE;
        StartCoroutine(checkMonsterState());
        StartCoroutine(MonsterAction());
    }
}


