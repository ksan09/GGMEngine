using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThisisMesh : MonoBehaviour
{
    MeshFilter      meshFilter;
    MeshRenderer    meshRenderer;

    private void Awake()
    {
        meshFilter      = GetComponent<MeshFilter>();
        meshRenderer    = GetComponent<MeshRenderer>();


    }

    private void Update()
    {
        Mesh mesh = new Mesh();

        Vector3[] verties   = new Vector3[3];
        Vector2[] uv        = new Vector2[3];
        int[] triangles     = new int[3];

        verties[0] = new Vector3(0, 0);
        verties[1] = new Vector3(0, 100);
        verties[2] = new Vector3(100, 100);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(0, 1);
        uv[2] = new Vector2(1, 1);

        mesh.vertices = verties;
        mesh.uv = uv;
        mesh.triangles = triangles;

        meshFilter.mesh = mesh;
    }
}
