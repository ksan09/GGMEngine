using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotationVal = 360;

    private void Update()
    {
        transform.Rotate(rotationVal * Time.deltaTime, 0, 0);
    }
}
