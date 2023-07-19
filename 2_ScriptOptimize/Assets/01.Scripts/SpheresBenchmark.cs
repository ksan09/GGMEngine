using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpheresBenchmark : MonoBehaviour
{
    public int numberOfSpheres = 100;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numberOfSpheres; i++)
        {
            GameObject sphereObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Renderer rend = sphereObj.GetComponent<Renderer>();
            rend.material = new Material(Shader.Find("Specular"));
            rend.material.color = Color.red;

            sphereObj.transform.position = Random.insideUnitSphere * 20;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        // 키 입력을 받아서 전체 스피어(구)를 이동시키는 코딩
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Transform[] spheres = FindObjectsOfType<Transform>();
            foreach(Transform t in spheres)
            {
                t.Translate(0, 0, 1f);
            }
        }

    }
}
