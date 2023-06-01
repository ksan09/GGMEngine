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


    public float traceDist = 10.0f; //���� �����Ÿ�
    public float attackDist = 2.0f;  //���� �����Ÿ�
    public bool isDie = false; //���� ��� ����

    private Transform playerTr;
    private NavMeshAgent agent;
    private Animator anim;

    //Animator �Ķ������ �ؽð� ����
    private readonly int hashTrace = Animator.StringToHash("IsTrace");
    private readonly int hashAttack = Animator.StringToHash("IsAttack");
    private readonly int hashDie= Animator.StringToHash("Death");

    private void Awake()
    {
        playerTr = GameManager.instance.PlayerTr; //�÷��̾� �ҷ�����
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        //NavMeshAgent�� �ڵ� ȸ�� ��� ��Ȱ��ȭ
        agent.updateRotation = false;
    }
    
   


    private void Update()
    {
        if(agent.remainingDistance >= 2.0f)
        {
            //�������� ����� ��ǥ �ӵ�. ������ �����̳� ��ֹ��� ���� ���� �ӵ��ʹ� ���̰� �� �� ����.
            //��ֹ� ȸ�Ǹ� ����� �̵� ����
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

    //������ ���¿� ���� ������ ���� ����
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
        pos.y += 1; //ray�� player�� ���� ���̱��� �ø��� ����
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
        //���� �Ÿ� ǥ��
        if (state == State.TRACE)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, traceDist);
        }

        //���� �Ÿ� ǥ��
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


