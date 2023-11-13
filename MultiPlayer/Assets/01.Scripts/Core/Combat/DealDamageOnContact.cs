
using Unity.Netcode;
using UnityEngine;

public class DealDamageOnContact : MonoBehaviour
{
    private int _damage = 10;

    private ulong _ownerClientID;

    public void SetDamage(int value)
    {
        _damage = value;
    }

    public void SetOwner(ulong ownerClientId)
    {
        _ownerClientID = ownerClientId;
    }

    //��TriggerEndter2d���� ���濡 �پ��ִ� Rigidbody�� �����;� ��
    //GetComponent Health �� ������
    // TakeDamage( _damage) �� �ο��ϸ� ��
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.attachedRigidbody is null) return;

        if (other.attachedRigidbody.TryGetComponent<NetworkObject>(out NetworkObject netObj))
        {
            if (netObj.OwnerClientId == _ownerClientID) return;
            // �ڱ� �ڽ��̸� �����ϰ� ����.
        }

        if (other.attachedRigidbody.TryGetComponent<Health>(out Health health))
        {
            health.TakeDamage(_damage);
        }
    }

}
