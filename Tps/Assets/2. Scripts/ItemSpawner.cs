using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] items; //������ ������
    public Transform playerTransform; //�÷��̾��� Ʈ������

    public float maxDistance = 5f; //�÷��̾� ��ġ���� �������� ��ġ�� �ִ� �ݰ�

    public float timeBetSpawnMax = 7f; // �ִ� �ð� ����
    public float timeBetSpawnMin = 2f; // �ּ� �ð� ����
    private float timeBetSpawn; // ���� ����

    private float lastSpawnTime; // ������ ���� ����

    private void Awake()
    {
        timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
        lastSpawnTime = 0;
    }

    private void Update()
    {
        if(lastSpawnTime + timeBetSpawn <= Time.time && playerTransform != null)
        {
            lastSpawnTime = Time.time;

            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);

            Spawn();
        }
    }

    private void Spawn()
    {
        Vector3 spawnPos =
            GetRandomPointOnNavMesh(playerTransform.position, maxDistance);

        spawnPos += Vector3.up * 0.5f;

        GameObject selectItem = items[Random.Range(0, items.Length)];
        GameObject item = Instantiate(selectItem, spawnPos, Quaternion.identity);

        Destroy(item, 5f);
    }

    private Vector3 GetRandomPointOnNavMesh(Vector3 center, float distance)
    {
        Vector3 randomPos = Random.insideUnitSphere * distance + center;

        NavMeshHit hit;

        NavMesh.SamplePosition(randomPos, out hit, distance, NavMesh.AllAreas);

        return hit.position;
    }

}
