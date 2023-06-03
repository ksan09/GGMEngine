using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private Transform _firePosTrm;
    private Light _muzzleLight;
    private LineRenderer _lineRenderer;

    [SerializeField]
    private LayerMask _whatIsEnemy;
    [SerializeField]
    private float _fireDistance;

    [SerializeField]
    private float _coolTime = 0.2f;
    [SerializeField]
    private int _gunDamage = 10;

    private float _lastFireTime;

    private void Awake()
    {
        _firePosTrm = transform.Find("FirePosition");
        _muzzleLight = transform.Find("FirePosition/MuzzleLight").GetComponent<Light>();
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void Fire()
    {
        if(_lastFireTime + _coolTime < Time.time)
        {
            _lastFireTime = Time.time;

            RaycastHit hit;

            _lineRenderer.enabled = true;
            _muzzleLight.enabled = true;
            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(0, _firePosTrm.position);
            Vector3 startPos = _firePosTrm.position + _firePosTrm.forward * -1f;
            //총 바로 앞에 있는 좀비를 맞추려면 총길이만큼 빼서 레이를 쏴야한다.
            if (Physics.Raycast(startPos, _firePosTrm.forward, out hit, _fireDistance, _whatIsEnemy))
            {
                _lineRenderer.SetPosition(1, hit.point);

                if(hit.collider.TryGetComponent(out IDamageable health))
                {
                    health.OnDamage(_gunDamage, hit.point, hit.normal);
                }
            }
            else
            {
                _lineRenderer.SetPosition(1, _firePosTrm.position + _firePosTrm.forward * _fireDistance);
            }

            StartCoroutine(EffectDelay());
        }
    }

    private IEnumerator EffectDelay()
    {
        yield return new WaitForSeconds(0.05f);
        _lineRenderer.enabled = false;
        _muzzleLight.enabled = false;
    }
}
