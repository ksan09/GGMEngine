using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
1. 데이터 추가 Add
2. 데이터를 반복한다 Iterate
3. 검색 Search
4. 삭제 Remove
*/

public class CollectionTest : MonoBehaviour
{
    const int numberOfTests = 10000;
    int[] inventory = new int[numberOfTests];
    Dictionary<int, int> inventoryDic = new Dictionary<int, int>();
    List<int> inventoryList = new List<int>();
    HashSet<int> inventoryHash = new HashSet<int>();

    private void Start()
    {
        AddValuesInArray();
        AddValuesInDic();
        AddValuesInList();
        AddValuesInHash();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            AddValuesInArray();
            AddValuesInDic();
            AddValuesInList();
            AddValuesInHash();
        }
    }

    private void AddValuesInHash()
    {
        for (int i = 0; i < numberOfTests; i++)
        {
            inventoryHash.Add(Random.Range(10, 100));
        }
    }

    private void AddValuesInList()
    {
        for (int i = 0; i < numberOfTests; i++)
        {
            inventoryList.Add(Random.Range(10, 100));
        }
    }

    private void AddValuesInDic()
    {
        for (int i = 0; i < numberOfTests; i++)
        {
            inventoryDic.Add(i, Random.Range(10, 100));
        }
    }

    private void AddValuesInArray()
    {
        for(int i = 0; i < numberOfTests; i++)
        {
            inventory[i] = Random.Range(10, 100);
        }
    }
}
