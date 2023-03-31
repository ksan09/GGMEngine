using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour, IDamageable
{
    [SerializeField]
    private Texture[] textures;
    private MeshRenderer render;

    public GameObject expEffect;
    private Rigidbody _barrelRb;
    private int hitCount = 0;


    private void Awake()
    {
        _barrelRb = GetComponent<Rigidbody>();


        render = GetComponentInChildren<MeshRenderer>();
        int idx = Random.Range(0, textures.Length);
        render.material.mainTexture = textures[idx];
    }

    public void OnDamage(float damage, Vector3 hitPosition, Vector3 hitNormal)
    {
        EffectManager.Instance.PlayHitEffect(hitPosition, hitNormal);
        if(++hitCount == 3)
        {
            ExpBarrel();
        }
        else
        {
            AttackBarrel(damage, -hitNormal);
        }
    }
    private void ExpBarrel()
    {
        //폭발효과 생성
        GameObject explosion = Instantiate(expEffect, transform.position, transform.rotation);

        Destroy(explosion, 2);
        _barrelRb.mass = 1.0f;
        _barrelRb.AddForce(Vector3.up * 50, ForceMode.Impulse);

        Destroy(gameObject, 2.0f);
    }

    private void AttackBarrel(float power, Vector3 dir)
    {
        _barrelRb.AddForce(power * dir, ForceMode.Impulse);
    }

}
