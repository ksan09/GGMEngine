using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaerTestScript : MonoBehaviour
{
    float _speed = 3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetMouseButton(0))
        {

            Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            dir.z = 0f;

            transform.position += (_speed * dir.normalized * Time.deltaTime);

        }
        


        Shader.SetGlobalVector("_ObjectPostion", new Vector4(transform.position.x, transform.position.y, transform.position.z, transform.localScale.x));

    }
}
