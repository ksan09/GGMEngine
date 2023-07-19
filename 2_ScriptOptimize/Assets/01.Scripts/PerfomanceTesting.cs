using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerfomanceTesting : MonoBehaviour
{
    const int numberOfTests = 5000;

    Transform testTrm;

    private void PerfomanceTest1()
    {
        for(int i = 0; i < numberOfTests; i++)
        {
            testTrm = GetComponent<Transform>();
        }
    }

    private void PerfomanceTest2()
    {
        for (int i = 0; i < numberOfTests; i++)
        {
            testTrm = GetComponent("Transform").transform;
        }
    }

    private void PerfomanceTest3()
    {
        for (int i = 0; i < numberOfTests; i++)
        {
            testTrm = (Transform)GetComponent(typeof(Transform));
        }
    }

    private void Update()
    {
        PerfomanceTest1();
        PerfomanceTest2();
        PerfomanceTest3();
    }

}
