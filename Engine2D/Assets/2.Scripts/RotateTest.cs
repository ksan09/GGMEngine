using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTest : MonoBehaviour
{

    public Transform Target;
    public float Radius = 1;
    public float RotateSpeed = 10f;

    private float currentAngle = 0f;
    private Vector2 dir = Vector2.right;

    // Update is called once per frame
    void Update()
    {

        currentAngle += RotateSpeed * Time.deltaTime;        
        dir = new Vector2(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad));

        Target.position = dir * Radius;

    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, Radius);

    }

}
