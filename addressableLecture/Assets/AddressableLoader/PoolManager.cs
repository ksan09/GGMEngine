using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class PoolManager
{
    public static PoolManager Instance;

    public Dictionary<string, Pool<PoolableMono>> _pools = new Dictionary<string, Pool<PoolableMono>>();

    private Transform _trmParent;

    public PoolManager(Transform trmParent)
    {
        _trmParent = trmParent;
    }

    public void ClearPools()
    {
        foreach(var pool in _pools)
        {
            pool.Value.Clear();
        }
        _pools.Clear();
    }

    public void CreatePool(string assetGUID, PoolableMono prefab, int count = 10)
    {
        Pool<PoolableMono> pool = new Pool<PoolableMono>(assetGUID, prefab, _trmParent, count);
        _pools.Add(assetGUID, pool);
    }

    public PoolableMono Pop(AssetReference assetReference)
    {
        if(_pools.ContainsKey(assetReference.AssetGUID))
        {
            PoolableMono item = _pools[assetReference.AssetGUID].Pop();
            item.Init();
            return item;
        }

        Debug.LogError("Prefab doesn't exist on pool");
        return null;
    }
    
    public void Push(PoolableMono obj)
    {
        _pools[obj.assetGUID].Push(obj);
    }
}
