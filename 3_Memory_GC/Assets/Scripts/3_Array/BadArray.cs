using System;
using UnityEngine;

public class BadArray : MonoBehaviour
{
    float[] randResultVal;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            randResultVal = RandomList(10000);
    }

    private float[] RandomList(int value)
    {
        float[] result = new float[value];
        for(int i = 0; i < value; ++i)
        {
            result[i] = UnityEngine.Random.Range(0.0f, 1.0f);
        }
        return result;
    }
}
