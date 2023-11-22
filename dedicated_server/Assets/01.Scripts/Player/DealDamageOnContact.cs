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

    // Ʈ���� �浹�� �߻����� ��
    // Health�� �����ͼ� TankDamage����

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(1);

        if(col.attachedRigidbody.TryGetComponent<Health>(out Health health))
        {
            health.TakeDamage(_damage);
        }
    }
}
