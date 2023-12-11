using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ItemManager : NetworkBehaviour
{
    public List<Transform> _spawnPointList;
    public GameObject _item;
    //public NetworkVariable<List<Item>> _items;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
            StartCoroutine(SpawnItemCo());
    }

    public override void OnNetworkDespawn()
    {
        StopAllCoroutines();
    }

    IEnumerator SpawnItemCo()
    {
        while(true)
        {
            yield return new WaitForSeconds(10f);

            int idx = Random.Range(0, _spawnPointList.Count);
            SpawnItemClientRpc(_spawnPointList[idx].position);
        }
    }

    [ClientRpc]
    private void SpawnItemClientRpc(Vector2 point)
    {
        SpawnItem(point);
    }

    private void SpawnItem(Vector2 pos)
    {
        var item = Instantiate(_item, pos, Quaternion.identity);
    }
}
