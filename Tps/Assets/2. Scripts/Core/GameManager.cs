using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Enemy Create info")]

    public List<Transform> points = new List<Transform>();
    public PoolableMono monster;
    public float createTime = 2.0f;
    public int maxMonster = 10;


    private void Awake()
    {
        if (instance == null)
            instance = this;

        PoolManager.Instance = new PoolManager(transform);
    }

    private void Start()
    {
        PoolManager.Instance.CreatePool(monster, maxMonster);

        Transform spawnPointGroup = GameObject.Find("SpawnPointGroup")?.transform;
        foreach (Transform point in spawnPointGroup)
            points.Add(point);

        InvokeRepeating("CreateMonster", 2.0f, createTime);

        HideCursor(true);
    }

    private void HideCursor(bool state)
    {
        Cursor.lockState = state ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !state;
    }

    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            HideCursor(false);
        }
        if(Input.GetMouseButtonDown(1))
        {
            HideCursor(true);
        }
    }

    public void CreateMonster()
    {
        int idx = UnityEngine.Random.Range(0, points.Count);

        PoolableMono _mob = PoolManager.Instance.Pop("Monster");

        _mob?.transform.SetPositionAndRotation(points[idx].position, points[idx].rotation);
    }
}
