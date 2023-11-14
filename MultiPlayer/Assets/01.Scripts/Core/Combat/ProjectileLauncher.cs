using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileLauncher : NetworkBehaviour
{
    [Header("참조 변수들")]
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private GameObject _serverProjectilePrefab;
    [SerializeField] private GameObject _clientProjectilePrefab;
    [SerializeField] private Collider2D _playerCollider;

    [Header("셋팅 값들")]
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

        if (Time.time < _prevFireTime + _fireCooltime) return; //쿨타임이 아직 남아있다.

        foreach(var trm in _firePosTrm)
        {
            PrimaryFireServerRPC(trm.position, trm.up);
            SpawnDummyProjectile(trm.position, trm.up);
        }
        _prevFireTime = Time.time;
    }

    [ServerRpc] // 서버에 있는 내 탱크의 이 매서드를 실행시키는 게 RPC콜이다.
    private void PrimaryFireServerRPC(Vector3 position, Vector3 dir)
    {
        var instance = Instantiate(_serverProjectilePrefab, position, Quaternion.identity);//서버만 가지고 있는거
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
        instance.transform.up = dir; //미사일을 해당방향으로 회전시키다
        //이건 2개의 컬라이더 간의 충돌은 무시한다.
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
        //터렛의 TurretPivot의 Turret을 찾아와서
        //FirePos의 위치에다가 새로운 게임 오브젝트를 만들어서
        //아까 찾은 Turret에 자식으로 걔를 붙여주면 된다.
        //자식으로 붙여준 게임오브젝트의 transform을 _firePosTrm 리스트에 딸깍 넣어주면 완성된다.
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
