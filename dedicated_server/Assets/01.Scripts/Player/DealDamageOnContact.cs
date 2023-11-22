using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageOnContact : MonoBehaviour
{
    private int _damage;
    private ulong _ownerClientID;

    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    public void SetOwner(ulong clientId)
    {
        _ownerClientID = clientId;
    }

    // 트리거 충돌이 발생했을 때
    // Health를 가져와서 TankDamage실행

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(1);

        if(col.attachedRigidbody.TryGetComponent<Health>(out Health health))
        {
            health.TakeDamage(_damage);
        }
    }
}
