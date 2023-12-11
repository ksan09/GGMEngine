using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ItemCollector : NetworkBehaviour
{
    [SerializeField]
    private ShootKnife _knife;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.TryGetComponent<Item>(out Item item))
        {
            _knife.GetItem(item.DamageUp, item.ScaleUp);
            Destroy(col.gameObject);
        }
    }
}
