
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class CoroutineCaching : MonoBehaviour
{
    /*
    �⺻ �ڷ�ƾ vs ĳ�� �ڷ�ƾ 
     
     */

    public int maxSpawnCount = 100;
    public float spawnDelay = 0.1f;
    public GameObject cubeObjPrefab;

    // �ڷ�ƾ ���� ������ ����
    private int spawnCount;
    private Vector3 randomPos;
    private GameObject newCube;

    Stopwatch stopwatch = new Stopwatch();

    private WaitForSeconds spawnWFS;
    private Coroutine spawnCoVal;

    private void Start()
    {
        spawnWFS = new WaitForSeconds(spawnDelay);
        spawnCoVal = null;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(spawnCoVal == null)
                spawnCoVal = StartCoroutine(spawnCo());
        }
    }

    IEnumerator spawnCo()
    {
        Debug.Log("ť�� ���� �׽�Ʈ ����!");

        for(int i = 0; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).gameObject);

        stopwatch.Reset();
        stopwatch.Start();

        spawnCount = maxSpawnCount;

        while(spawnCount > 0)
        {
            randomPos = new Vector3(Random.value, Random.value, Random.value);
            newCube = Instantiate(cubeObjPrefab, randomPos, Quaternion.identity);
            newCube.GetComponent<Renderer>().material.color = Random.ColorHSV();
            newCube.transform.SetParent(transform);

            yield return spawnWFS;

            spawnCount--;
        }

        stopwatch.Stop();
        spawnCoVal = null;

        Debug.Log($"{stopwatch.ElapsedMilliseconds / 1000} �� �ɸ�");
    }
}
