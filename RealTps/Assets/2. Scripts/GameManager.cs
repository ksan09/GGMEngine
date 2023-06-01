using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PoolListSO initList;

    [Header("Enemy Create Info")]
    //���Ͱ� ������ ��ġ�� ������ List Ÿ�� ����
    public List<Transform> points = new List<Transform>();
    //���͸� �̸� ������ ������ ����Ʈ �ڷ���
    public List<GameObject> monsterPool = new List<GameObject>();
    public GameObject monster; //���� �������� ������ ����
    public float createTime = 3.0f;
    //������Ʈ Ǯ�� ������ ������ �ִ� ����
    public int maxMonster = 10;
   

    [Header("Taret GameObjects")]
    public GameObject player;
    
    [HideInInspector]
    public static GameManager instance = null;

   // [SerializeField]

    public int score; //����

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

    //������ ���� ���θ� ������ ��� ����
    public bool isGameOver = false;
    //������ ���� ���θ� ������ ������Ƽ
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
            Debug.LogWarning("���: �ټ��� ���ӸŴ��� ������Ʈ");
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

        //��ġ ����
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
        //���� �ð� ������ 1������ Ȱ��ȭ - 3�ʸ��� 1������ ����
       InvokeRepeating("CreateMonster", 2.0f, createTime); //ȣ���� �Լ�, ���ð�(�����Ŀ� �Լ��� �����ұ�), ȣ�Ⱓ��(�ð� �������� �Լ� �ݺ��� ȣ��)

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
        // ���� ������ �ƴ� ���¿����� ���� ���� ����
        if (!isGameOver)
        {
            // ���� �߰�
            score += newScore;
            UIManager.Instance.UpdateScoreText(score);
        }
    }

    public void EndGame()
    {
        // ���� ���� ���¸� ������ ����
        isGameOver = true;
        // ���� ���� UI�� Ȱ��ȭ
        UIManager.Instance.SetActiveGameoverUI(true);
    }

}
