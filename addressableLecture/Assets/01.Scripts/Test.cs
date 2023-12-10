using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Test : MonoBehaviour
{
    [SerializeField] private AssetReference refer;
    private List<GameObject> objList = new();

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            LoadLevel();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            foreach (var obj in objList)
                Destroy(obj);
            refer.ReleaseAsset();
            objList.Clear();
        }
    }

    private async void LoadLevel()
    {
        if(!refer.IsValid())
        {
            GameObject levelObj = await refer.LoadAssetAsync<GameObject>().Task;
            
            objList.Add(Instantiate(levelObj, Vector3.zero, Quaternion.identity));
        }
        else
        {

            objList.Add(Instantiate(refer.Asset, Vector3.zero, Quaternion.identity) as GameObject);
        }
    }

}
