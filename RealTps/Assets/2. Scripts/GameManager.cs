using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PoolListSO initList;

    [Header("Enemy Create Info")]
    //몬스터가 출현할 위치를 저장할 List 타입 변수
    public List<Transform> points = new List<Transform>();
    //몬스터를 미리 생성해 저장할 리스트 자료형
    public List<GameObject> monsterPool = new List<GameObject>();
    public GameObject monster; //몬스터 프리팹을 연결할 변수
    public float createTime = 3.0f;
    //오브젝트 풀에 생성할 몬스터의 최대 개수
    public int maxMonster = 10;
   

    [Header("Taret GameObjects")]
    public GameObject player;
    
    [HideInInspector]
    public static GameManager instance = null;

   // [SerializeField]

    public int score; //점수

    private Transform playerTr = null;
    public Transform PlayerTr
    {
        get
        {
            if(playerTr == null)
            {
                playerTr = GameObject.FindGameObjectWithTag("Player").transform;
            }
            return playerTr;
        }
    }

    //게임의 종료 여부를 저장할 멤버 변수
    public bool isGameOver = false;
    //게임의 종료 여부를 저장할 프로퍼티
    public bool IsGameVOver
    {
        get { return isGameOver; }
        set { 
            isGameOver = value;
        if (isGameOver) CancelInvoke("CreateMonster");
        }
    }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("경고: 다수의 게임매니저 오브젝트");
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        CreatePool();

    }

    private void CreatePool()
    {
        PoolManager.Instance = new PoolManager(transform);
        initList.Pairs.ForEach(p =>
        {
            PoolManager.Instance.CreatePool(p.Prefab, p.count);
        });
    }

    private void CreateMonster()
    {
       MonsterCtrl m = PoolManager.Instance.Pop("Monster 1") as MonsterCtrl;

        //위치 지정
        Transform spawnPointGroup = GameObject.Find("SpawnPointGroup")?.transform;
        foreach (Transform point in spawnPointGroup)
        {
            points.Add(point);
        }

        int idx = Random.Range(0, points.Count);
        m?.transform.SetPositionAndRotation(points[idx].position, points[idx].rotation);

    }

    private void Start()
    {   
        //일정 시간 지나면 1마리씩 활성화 - 3초마다 1마리씩 증가
       InvokeRepeating("CreateMonster", 2.0f, createTime); //호출한 함수, 대기시간(몇초후에 함수를 실행할까), 호출간격(시간 간격으로 함수 반복해 호출)

        HideCursor(true);

    }
    private void HideCursor(bool state)
    {
        Cursor.lockState = state ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !state;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HideCursor(false);
        }
        if (Input.GetMouseButtonDown(1))
        {
            HideCursor(true);
        }
    }

    public void AddScore(int newScore)
    {
        // 게임 오버가 아닌 상태에서만 점수 증가 가능
        if (!isGameOver)
        {
            // 점수 추가
            score += newScore;
            UIManager.Instance.UpdateScoreText(score);
        }
    }

    public void EndGame()
    {
        // 게임 오버 상태를 참으로 변경
        isGameOver = true;
        // 게임 오버 UI를 활성화
        UIManager.Instance.SetActiveGameoverUI(true);
    }

}
