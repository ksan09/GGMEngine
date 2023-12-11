using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Item : NetworkBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private CircleCollider2D _collider2D;

    private int _damageValue = 2;
    private float _scaleValue = 0.5f;

    public int DamageUp => _damageValue;
    public float ScaleUp => _scaleValue;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider2D = GetComponent<CircleCollider2D>();
    }

    public override void OnNetworkSpawn()
    {
        
    }

}
