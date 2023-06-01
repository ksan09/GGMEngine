using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBB : IEnumerator
{
    public object Current { get; set; }

    private List<int> _list = new List<int>();
    private int now = 0;

    public bool MoveNext()
    {
        if (_list.Count < now)
        {
            Current = _list[now];
            now++;
            return true;
        }
        else
            return false;
            
        
    }

    public void Reset()
    {
        now = 0;
    }
}

public class Test22 : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator tranEnum;
    void Start()
    {
        tranEnum = transform.GetEnumerator();
    }

    IEnumerator AAA()
    {
        yield return 1;
        yield return 2;
        yield return 3;
        yield return 4;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(tranEnum.MoveNext())
                Debug.Log(tranEnum.Current);
        }
    }
}
