using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class TestUV : MonoBehaviour
{
    private MeshFilter _meshFilter;

    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
    }

    private void Start()
    {
        Mesh mesh = new Mesh();

        Vector3[] verties = new Vector3[3];
        int[] triangles = new int[3];

        verties[0] = new Vector3(0, 0);
        verties[1] = new Vector3(0, 4);
        verties[2] = new Vector3(4, 4);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        mesh.vertices = verties;
        mesh.triangles = triangles;678950858594

        _meshFilter.mesh = mesh;
    }

}
