using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyListCreater : MonoBehaviour
{
    public int numberOfLists;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            CreateLists();
    }

    void CreateLists()
    {
        for(int i = 0; i < numberOfLists; i++)
        {
            List<string> nameList = new List<string>(numberOfLists);
        }
    }
}
