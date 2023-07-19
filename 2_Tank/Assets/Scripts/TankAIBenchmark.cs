using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAIBenchmark : MonoBehaviour
{
    GameObject[] tanks;
    public int numberOfTanks;
    public GameObject tankPrefab;

    // Start is called before the first frame update
    void Start()
    {
        tanks = new GameObject[numberOfTanks];
        for (int i = 0; i < numberOfTanks; i++)
        {
            tanks[i] = Instantiate(tankPrefab);
            tanks[i].transform.position = new Vector3(Random.Range(-50,50), 0, Random.Range(-50,50));
        }
    }

    private int interval = 3;

    // Update is called once per frame
    void Update()
    {
        if(Time.frameCount % interval == 0)
        {
            // 값비싼 함수 호출
            // 갱신 주기가 매프레임마다 필요치 않는 함수들
        }

        foreach (GameObject t in tanks)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                t.transform.LookAt(player.transform.position);
                t.transform.Translate(0, 0, 0.05f);
            }
        } 
    }
}
