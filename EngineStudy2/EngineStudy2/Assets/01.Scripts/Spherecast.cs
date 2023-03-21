using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Spherecast : MonoBehaviour
{
    float distance = 10;
    private LayerMask mask;

    private void OnDrawGizmos()
    {
        RaycastHit hit;

        mask = LayerMask.GetMask("Cube");
        Vector3 dir = transform.forward;
        bool isHit = Physics.SphereCast(transform.position, transform.lossyScale.x/2, dir, out hit, distance, mask);
        //bool isHit = Physics.BoxCast(transform.position, transform.lossyScale/2, dir, out hit, Quaternion.identity, distance);
        Debug.Log(isHit);
        if (isHit)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, transform.forward * hit.distance);
            Gizmos.DrawWireSphere(transform.position + transform.forward * hit.distance, transform.lossyScale.x / 2);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.forward * distance);
        }
    }
}
