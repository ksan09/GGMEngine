using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotRaycast : MonoBehaviour
{
    // Raycast
    [SerializeField]
    private LayerMask mask;

    [SerializeField]
    private float distance = 0;


    //private void Update()
    //{
    //    ShotRay();
    //}

    //private void ShotRay()
    //{
    //    //
    //}

    private void OnDrawGizmos()
    {
        RaycastHit hit;
        Vector3 dir = transform.forward;
        bool isHit = Physics.Raycast(transform.position, dir, out hit, distance, mask);
        //bool isHit = Physics.BoxCast(transform.position, transform.lossyScale/2, dir, out hit, Quaternion.identity, distance);
        Debug.Log(isHit);
        if (isHit)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, transform.forward * hit.distance);


            //MeshRenderer mr = hit.transform.GetComponent<MeshRenderer>();
            //MeshRenderer myMr = GetComponent<MeshRenderer>();
            //if(mr != null && myMr != null)
            //    mr.material = myMr.material;
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.forward * distance);
        }
        Gizmos.DrawWireCube(transform.position + transform.forward*hit.distance, transform.localScale);
    }
}
