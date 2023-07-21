using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodArray : MonoBehaviour
{
    public int count = 10000;
    float[] randResultVal;

    private void Start()
    {
        randResultVal = new float[count];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            RandomList(randResultVal);
    }

    private void RandomList(float[] arrayToFill)
    {
        for (int i = 0; i < count; ++i)
        {
            arrayToFill[i] = UnityEngine.Random.Range(0.0f, 1.0f);
        }
    }
}
