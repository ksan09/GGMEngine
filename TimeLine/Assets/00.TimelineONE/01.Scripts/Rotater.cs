using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    public float rotationVal = 360;

    private void Update()
    {
        transform.Rotate(rotationVal * Time.deltaTime, 0, 0);
    }
}
