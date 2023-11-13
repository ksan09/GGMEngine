
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

    //온TriggerEndter2d에서 상대방에 붙어있는 Rigidbody를 가져와야 해
    //GetComponent Health 를 가져와
    // TakeDamage( _damage) 을 부여하면 끝
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.attachedRigidbody is null) return;

        if (other.attachedRigidbody.TryGetComponent<NetworkObject>(out NetworkObject netObj))
        {
            if (netObj.OwnerClientId == _ownerClientID) return;
            // 자기 자신이면 무시하고 전진.
        }

        if (other.attachedRigidbody.TryGetComponent<Health>(out Health health))
        {
            health.TakeDamage(_damage);
        }
    }

}
