using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [Range(0, 360)]
    public float viewAngle;
    public float viewRadius;

    [SerializeField] private LayerMask _enemyMask;
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private float _enemyFindDelay = 0.3f;
    public List<Transform> visibleTargets = new List<Transform>();

    [SerializeField] private float _meshResolution;

    private void Start()
    {
        StartCoroutine(FindEnemyWithDelay(_enemyFindDelay));
    }

    private void Update()
    {
        DrawFieldOfView();
    }

    private void DrawFieldOfView()
    {
        //
        int stepCount = Mathf.RoundToInt(_meshResolution * viewAngle);
        float stepAngleSize = viewAngle / stepCount;

        Vector3 pos = transform.position;
        //나의 위치부터
        for(int i = 0; i < stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle * 0.5f
                + stepAngleSize * i;
            //현재 로테이션 어쩌구 저쩌구

            Debug.DrawLine(pos, pos + DirFromAngle(angle, true) * viewRadius, Color.red);
        }
    }

    private void FindVisibleEnemies()
    {
        visibleTargets.Clear();

        Collider[] enemiesInView = new Collider[6];
        int cnt = Physics.OverlapSphereNonAlloc(transform.position, viewRadius, enemiesInView, _enemyMask);

        for(int i = 0; i < cnt; i++)
        {
            Transform enemy = enemiesInView[i].transform;
            Vector3 dir = enemy.position - transform.position;

            if(Vector3.Angle(transform.forward, dir) < viewAngle * 0.5f)
            {
                if (!Physics.Raycast(transform.position, dir, dir.magnitude, _obstacleMask))
                {
                    visibleTargets.Add(enemy);
                }
            }
        }
    }

    public Vector3 DirFromAngle(float degree, bool anglesGlobal)
    {
        //global local
        if(!anglesGlobal)
        {
            degree += transform.eulerAngles.y;
        }

        float rad = degree * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad));
    }

    IEnumerator FindEnemyWithDelay(float delay)
    {
        var time = new WaitForSeconds(delay);
        while (true)
        {
            FindVisibleEnemies();
            yield return time;
        }
    }
}
