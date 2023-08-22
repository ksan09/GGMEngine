using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileLuncher : NetworkBehaviour
{
    [Header("참조 변수들")]
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Transform _projectileSpawnTrm;
    [SerializeField] private GameObject _serverProjectilePrefab;
    [SerializeField] private GameObject _clientProjectilePrefab;
    [SerializeField] private Collider2D _playerCollider;

    [Header("셋팅 값들")]
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private float _fireCooltime;

    private bool _shouldFire;
    private float _prevFireTime;

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

        if (Time.time < _prevFireTime + _fireCooltime) return;

        PrimaryFireServerRPC(_projectileSpawnTrm.position, _projectileSpawnTrm.up);
        SpawnDummyProjectile(_projectileSpawnTrm.position, _projectileSpawnTrm.up);
        _prevFireTime = Time.time;
    }

    [ServerRpc] // 서버에 있는 내 탱크의 이 메서드를 실행시키는 게 RPC콜이다.
    private void PrimaryFireServerRPC(Vector3 position, Vector3 dir)
    {
        //서버만 가지고 있는 Obj
        var instance = Instantiate(_serverProjectilePrefab, position, Quaternion.identity);
        instance.transform.up = dir; // 미사일 회전
        //2개의 콜라이더 간의 충돌은 무시한다.
        Physics2D.IgnoreCollision(_playerCollider, instance.GetComponent<Collider2D>());

        if (instance.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody))
        {
            rigidbody.velocity = rigidbody.transform.up * _projectileSpeed;
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
        instance.transform.up = dir; // 미사일 회전
        //2개의 콜라이더 간의 충돌은 무시한다.
        Physics2D.IgnoreCollision(_playerCollider, instance.GetComponent<Collider2D>());

        OnFire?.Invoke();
        if(instance.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody))
        {
            rigidbody.velocity = rigidbody.transform.up * _projectileSpeed;
        }
    }
}
