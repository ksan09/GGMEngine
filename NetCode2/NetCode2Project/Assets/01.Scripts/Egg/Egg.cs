using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using Unity.VisualScripting;

[RequireComponent(typeof(Rigidbody2D))]
public class Egg : NetworkBehaviour
{
    [SerializeField] private float _bounceVelocity;
    [SerializeField] private float _waitingTime = 2f;
    private Rigidbody2D _rigidbody;

    public static Action OnHit;
    public static Action OnFallInWater;

    private const string waterTag = "Water";

    private bool _isAlive = true;
    private float _gravityScale = 0;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _gravityScale = _rigidbody.gravityScale;
        _rigidbody.gravityScale = 0;
    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        StartCoroutine(WaitAndFall());
    }

    private IEnumerator WaitAndFall()
    {
        yield return new WaitForSeconds(_waitingTime);
        _rigidbody.gravityScale = _gravityScale;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!IsServer) return;
        if (!_isAlive) return;

        if(col.collider.TryGetComponent<PlayerMovement>(out PlayerMovement player))
        {
            if (col.contacts.Length > 0)
            {
                Vector2 normal = col.GetContact(0).normal;
                Bounce(normal);
                OnHit?.Invoke(); //턴을 넘기기 위해서 이걸 발생시킨다.
            }
        }
    }

    private void Bounce(Vector2 normal)
    {
        _rigidbody.velocity = normal * _bounceVelocity;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!IsServer) return;
        if (!_isAlive) return;

        if (col.CompareTag(waterTag))
        {
            _isAlive = false;
            OnFallInWater?.Invoke();
        }
    }

    public void ResetToStartPosition(Vector3 eggStartPosition)
    {
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.angularVelocity = 0;
        _rigidbody.gravityScale = 0;
        transform.SetPositionAndRotation(eggStartPosition, Quaternion.identity);
        _isAlive = true;
        StartCoroutine(WaitAndFall());
    }

}

