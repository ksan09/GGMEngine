using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class BulletTrail : PoolableMono
{
    [SerializeField] private float _lifeTime = 0.2f;
    private TrailRenderer _trailRenderer;

    private void Awake()
    {
        _trailRenderer = GetComponent<TrailRenderer>();
    }

    public override void Init()
    {
        _trailRenderer.Clear();
    }

    public void DrawTrail(Vector3 startPos, Vector3 endPos, float lifeTime)
    {
        _trailRenderer.AddPosition(startPos);
        _trailRenderer.AddPosition(endPos);
        _trailRenderer.time = lifeTime;

        StartCoroutine(LifeTimeCoroutine());
    }

    private IEnumerator LifeTimeCoroutine()
    {
        yield return new WaitForSeconds(_lifeTime);
        PoolManager.Instance.Push(this);
    }



}
