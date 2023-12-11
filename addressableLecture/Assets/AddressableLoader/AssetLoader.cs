using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class AssetLoader : MonoBehaviour
{
    [SerializeField] private AssetLoaderSO _assetLoaderSO;

    public AssetLoaderSO Assets => _assetLoaderSO;
    public delegate void InvokeMessage(string message);
    public delegate void Notify();

    public static event InvokeMessage OnCategoryMessage;    //
    public static event InvokeMessage OnDescMessage;        //

    public static event Notify OnLoadComplete;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private async void Start()
    {
        var totalCount = _assetLoaderSO.totalCount;
        OnCategoryMessage?.Invoke($"Loading {totalCount} Assets...");
        await LoadAsset();
        OnCategoryMessage?.Invoke($"Make pooling items...");
        await MakePooling();

        OnLoadComplete?.Invoke();
    }

    private async Task LoadAsset()
    {
        foreach(var refer in _assetLoaderSO.loadingList)
        {
            var asset = await refer.LoadAssetAsync<GameObject>().Task;
            OnDescMessage?.Invoke($"Loading... {asset.name}");
            _assetLoaderSO.LoadingComplete(refer, asset.name);
        }

        foreach (var refer in _assetLoaderSO.poolingList)
        {
            var asset = await refer.assetRef.LoadAssetAsync<GameObject>().Task;
            OnDescMessage?.Invoke($"Loading... {asset.name}");
            _assetLoaderSO.LoadingComplete(refer.assetRef, asset.name);
        }
    }

    private async Task MakePooling()
    {
        if(PoolManager.Instance == null)
            PoolManager.Instance = new PoolManager(transform);

        foreach (var refer in _assetLoaderSO.poolingList)
        {
            var prefab = refer.assetRef.Asset.GetComponent<PoolableMono>();
            if(prefab == null)
            {
                Debug.LogWarning($"{refer.assetRef.Asset.name} doesn't have PoolableMono Component, skip it");
                continue;
            }
            OnDescMessage?.Invoke($"loading.. {refer.assetRef.Asset.name}");
            await Task.Delay(1); // UI가 반영되는 시간...
            PoolManager.Instance.CreatePool(refer.assetRef.AssetGUID, prefab, refer.count);
        }
        
    }


}
