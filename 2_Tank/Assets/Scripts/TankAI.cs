using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAI : MonoBehaviour
{
    Transform[] tanks;

    public int numberOfTanks;

    public GameObject tankPrefab;
    public GameObject player;

    private void Start()
    {
        tanks = new Transform[numberOfTanks];
        GameObject tank;

        for (int i = 0; i < numberOfTanks; i++)
        {
            tank = Instantiate(tankPrefab);
            tank.transform.position = new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50));
            tanks[i] = tank.transform;
        }
    }

    private void Update()
    {
        foreach (Transform t in tanks)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                t.LookAt(player.transform.position);
                t.Translate(0, 0, 0.05f);
            }
        }
    }
}
