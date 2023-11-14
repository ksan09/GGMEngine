using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileLauncher : NetworkBehaviour
{
    [Header("���� ������")]
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private GameObject _serverProjectilePrefab;
    [SerializeField] private GameObject _clientProjectilePrefab;
    [SerializeField] private Collider2D _playerCollider;

    [Header("���� ����")]
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private float _fireCooltime;

    private bool _shouldFire;
    private float _prevFireTime;
    private NetworkVariable<int> _damage = new NetworkVariable<int>(10);
    private List<Transform> _firePosTrm = new List<Transform>();

    public UnityEvent OnFire;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        _inputReader.PrimaryFireEvent += HandleFire;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;
        _inputReader.PrimaryFireEvent -= HandleFire;
    }

    private void HandleFire(bool button)
    {
        _shouldFire = button;
    }

    private void Update()
    {
        if (!IsOwner) return;
        if (!_shouldFire) return;

        if (MapManager.Instance.InSafeZone(transform.position))
            return;

        if (Time.time < _prevFireTime + _fireCooltime) return; //��Ÿ���� ���� �����ִ�.

        foreach(var trm in _firePosTrm)
        {
            PrimaryFireServerRPC(trm.position, trm.up);
            SpawnDummyProjectile(trm.position, trm.up);
        }
        _prevFireTime = Time.time;
    }

    [ServerRpc] // ������ �ִ� �� ��ũ�� �� �ż��带 �����Ű�� �� RPC���̴�.
    private void PrimaryFireServerRPC(Vector3 position, Vector3 dir)
    {
        var instance = Instantiate(_serverProjectilePrefab, position, Quaternion.identity);//������ ������ �ִ°�
        instance.transform.up = dir;
        Physics2D.IgnoreCollision(_playerCollider, instance.GetComponent<Collider2D>());
        if (instance.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody))
        {
            rigidbody.velocity = rigidbody.transform.up * _projectileSpeed;
        }

        if (instance.TryGetComponent<DealDamageOnContact>(out DealDamageOnContact dealContact))
        {
            dealContact.SetDamage(_damage.Value);
            dealContact.SetOwner(OwnerClientId);
        }

        SpawnDummyProjectileClientRPC(position, dir);
    }

    [ClientRpc]
    private void SpawnDummyProjectileClientRPC(Vector3 position, Vector3 dir)
    {
        if (IsOwner) return;

        SpawnDummyProjectile(position, dir);
    }

    private void SpawnDummyProjectile(Vector3 position, Vector3 dir)
    {
        var instance = Instantiate(_clientProjectilePrefab, position, Quaternion.identity);
        instance.transform.up = dir; //�̻����� �ش�������� ȸ����Ű��
        //�̰� 2���� �ö��̴� ���� �浹�� �����Ѵ�.
        Physics2D.IgnoreCollision(_playerCollider, instance.GetComponent<Collider2D>());

        OnFire?.Invoke();
        if (instance.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody))
        {
            rigidbody.velocity = rigidbody.transform.up * _projectileSpeed;
        }
    }

    public void SetDamage(int damage)
    {
        _damage.Value = damage;
    }

    public void SetFirePosition(Vector3[] firePos)
    {
        //�ͷ��� TurretPivot�� Turret�� ã�ƿͼ�
        //FirePos�� ��ġ���ٰ� ���ο� ���� ������Ʈ�� ����
        //�Ʊ� ã�� Turret�� �ڽ����� �¸� �ٿ��ָ� �ȴ�.
        //�ڽ����� �ٿ��� ���ӿ�����Ʈ�� transform�� _firePosTrm ����Ʈ�� ���� �־��ָ� �ϼ��ȴ�.
        Transform parent = transform.Find("TurretPivot/Turret");
        foreach(var offset in firePos)
        {
            GameObject firePoint = new GameObject();
            firePoint.transform.SetParent(parent);
            firePoint.transform.localPosition = offset;

            _firePosTrm.Add(firePoint.transform);
        }
        
    }
}
