using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] items; //생성할 아이템
    public Transform playerTransform; //플레이어의 트랜스폼

    public float maxDistance = 5f; //플레이어 위치에서 아이템이 배치될 최대 반경

    public float timeBetSpawnMax = 7f; // 최대 시간 간격
    public float timeBetSpawnMin = 2f; // 최소 시간 간격
    private float timeBetSpawn; // 생성 간격

    private float lastSpawnTime; // 마지막 생성 시점

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
