using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    private Transform _playerTrm;
    public GameObject GoalObj;
    public int monsterCount;

    public Transform PlayerTrm
    {
        get
        {
            if(_playerTrm == null)
            {
                _playerTrm = GameObject.FindGameObjectWithTag("Player").transform;
            }
            return _playerTrm;
        }
    }

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("Multiple GameManager is running");
        }
        Instance = this;

        monsterCount = FindObjectsOfType<MonsterCtrl>().Length;
    }

    public void KillMonster()
    {
        monsterCount--;
        if (monsterCount == 0)
            GoalObj.SetActive(false);
    }
}
