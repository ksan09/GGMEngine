using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T : PoolableMono
{
    private Stack<T> _pool = new Stack<T> ();
    private T _prefab; // 오리지널 프리팹 저장, 그래야 모자랄 때 만들어 쓸 수 있으니까
    private Transform _parent; // 부모도 저장, 그래야 모자랄 때 만든 거를 부모 설정 가능하니까

    public Pool(T prefab, Transform parent, int count = 10)
    {
        _prefab = prefab;
        _parent = parent;

        for(int i = 0; i < count; i++)
        {
            T obj = GameObject.Instantiate (_prefab, parent);
            obj.gameObject.name = obj.gameObject.name.Replace("(Clone)", "");
            obj.gameObject.SetActive (false);
            _pool.Push(obj);
        }
    }

    public T Pop()
    {
        T obj = null;
        if(_pool.Count <= 0)
        {
            obj = GameObject.Instantiate(_prefab, _parent);
            obj.gameObject.name = obj.gameObject.name.Replace("(Clone)", "");
        } else
        {
            obj = _pool.Pop();
            obj.gameObject.SetActive(true);
        }
        return obj;
    }

    public void Push(T obj)
    {
        obj.gameObject.SetActive (false);
        _pool.Push (obj);
    }

}
