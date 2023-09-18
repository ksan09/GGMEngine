using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float distance;
        public float angle;
    }






    [Range(0, 360)]
    public float viewAngle;
    public float viewRadius;

    [SerializeField] private LayerMask _enemyMask;
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private float _enemyFindDelay = 0.3f;
    public List<Transform> visibleTargets = new List<Transform>();

    [SerializeField] private float _meshResolution;

    private MeshFilter _viewMeshFilter;
    private Mesh _viewMesh;

    private void Awake()
    {
        _viewMeshFilter = transform.Find("ViewVisualize")
            .GetComponent<MeshFilter>();

        _viewMesh = new Mesh();
        _viewMesh.name = "LeeJuneYi";
        _viewMeshFilter.mesh = _viewMesh;
    }

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

        List<Vector3> viewPoints = new List<Vector3>();
        //나의 위치부터
        for(int i = 0; i < stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle * 0.5f
                + stepAngleSize * i;
            //현재 로테이션 어쩌구 저쩌구

            var info = ViewCast(angle);
            viewPoints.Add(info.point);
        }
        int vertexCnt = viewPoints.Count + 1;
        Vector3[] verties = new Vector3[vertexCnt];
        int[] triangles = new int[(vertexCnt - 2) * 3];
        verties[0] = transform.InverseTransformPoint(transform.position);
        for (int i = 0; i < viewPoints.Count; ++i)
            verties[i+1] = transform.InverseTransformPoint(viewPoints[i]);
        int trianglesIdx = 0;
        for (int i = 0; i < viewPoints.Count - 1; ++i)
        {
            triangles[trianglesIdx++] = 0;
            triangles[trianglesIdx++] = i+1;
            triangles[trianglesIdx++] = i+2;
        }

        _viewMesh.vertices = verties;
        _viewMesh.triangles = triangles;
        _viewMesh.RecalculateNormals();
    }

    private ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        if(Physics.Raycast(transform.position, dir, out RaycastHit hit, viewRadius, _obstacleMask))
        {
            return new ViewCastInfo { hit = true, 
                point = hit.point, distance = hit.distance, angle = globalAngle};
        }
        else
        {
            return new ViewCastInfo { hit = false, 
                point = transform.position + dir * viewRadius, distance = viewRadius, angle = globalAngle};

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
