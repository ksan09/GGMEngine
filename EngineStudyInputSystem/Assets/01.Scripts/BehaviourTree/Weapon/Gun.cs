using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Transform _firePosTrm;
    [SerializeField] private float _coolTime;
    [SerializeField] private int _maxAmmo;
    [SerializeField] private float _reloadTime;
    [SerializeField] private float _maxDistance;

    private float _lastFireTime = 0;
    private float _currentCoolTime;
    private bool _isReload = false;
    private LineRenderer _lineRenderer;

    private int _ammo;
    public int Ammo => _ammo;
    public bool isReload => _isReload;
    public bool needReload => (_ammo <= 0);

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 2;
        _lineRenderer.startWidth = 0.1f;
        _lineRenderer.endWidth = 0.1f;
        _lineRenderer.enabled = false;

        _ammo = _maxAmmo;
        CalcNextCoolTime();


    }

    private void CalcNextCoolTime()
    {
        _currentCoolTime = _coolTime + Random.Range(-0.2f, 0.2f);
    }

    private void AmmoCount()
    {
        Debug.Log($"남은 총알 개수: {Ammo}");

    }

    public void Reload()
    {
        if (_isReload) return;
        StartCoroutine(ReloadCo());
    }

    IEnumerator ReloadCo()
    {
        _isReload = true;
        yield return new WaitForSeconds(_reloadTime);
        _ammo = _maxAmmo;
        _isReload = false;
        AmmoCount();
    }

    public bool Fire()
    {
        if (_lastFireTime + _currentCoolTime < Time.time && _ammo > 0)
        {
            _lastFireTime = Time.time;
            CalcNextCoolTime();

            RaycastHit hit;
            bool isHit = Physics.Raycast(_firePosTrm.position,
                _firePosTrm.forward, out hit, _maxDistance);
            _lineRenderer.SetPosition(0, _firePosTrm.position);
            _lineRenderer.SetPosition(1, isHit ? hit.point : 
                _firePosTrm.position + _firePosTrm.forward * _maxDistance);

            _lineRenderer.enabled = true;
            StartCoroutine(LineBlink(0.2f));
            _ammo--;
            AmmoCount();

            return true;
        }
        return false;
    }

    IEnumerator LineBlink(float time)
    {
        yield return new WaitForSeconds(time);
        _lineRenderer.enabled = false;
    }
}
