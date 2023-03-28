using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum State
    {
        Ready,
        Empty,
        Reloading
    }
    public State state { get; private set; }

    


    #region ����
    public Transform firePosition; //�Ѿ˳����� ��ġ�� ����
    public ParticleSystem muzzleFlashEffect;
    public float bulletLineEffectTime = 0.03f;

    private AudioSource _audioSource;
    public AudioClip _shootClip;
    public AudioClip _reloadClip;

    private LineRenderer bulletLineRenderer;
    public float damage = 5;

    public float fireDistance = 100f; //�߻簡�� �Ÿ�

    public int magCapacity = 30; //źâ �뷮
    public int magAmmo; //���� źâ�� �ִ� ź���
    public float timeBetFire = 0.12f; //ź�� �߻� ����
    public float reloadTime = 1.8f; //������ �ҿ�ð�
    private float lastFireTime; //���� ���������� �߻��� �ð�
    #endregion

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        bulletLineRenderer = GetComponent<LineRenderer>();
        bulletLineRenderer.positionCount = 2;
        bulletLineRenderer.enabled = false;
    }
    private void Start()
    {
        magAmmo = magCapacity;
        state = State.Ready;
        lastFireTime = 0f;
    }

    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        _audioSource.clip = _shootClip;
        _audioSource.Play();

        muzzleFlashEffect.Play();
        bulletLineRenderer.SetPosition(0, firePosition.position);
        bulletLineRenderer.SetPosition(1, hitPosition);
        bulletLineRenderer.enabled = true;

        yield return new WaitForSeconds(bulletLineEffectTime);

        bulletLineRenderer.enabled = false;
    }
    private IEnumerator ReloadRoutine()
    {
        _audioSource.clip = _reloadClip;
        _audioSource.Play();

        state = State.Reloading;
        yield return new WaitForSeconds(reloadTime);
        magAmmo = magCapacity;
        state = State.Ready;
    }

    public bool Fire()
    {
        if ( state == State.Ready && Time.time >= lastFireTime + timeBetFire)
        {
            lastFireTime = Time.time;
            Shot();
        }

        return false;
    }

    private void Shot()
    {
        //����ĳ��Ʈ
        RaycastHit hit;
        Vector3 hitPosition = Vector3.zero;

        if(Physics.Raycast(firePosition.position, firePosition.forward, out hit, fireDistance))
        {
            var target = hit.collider.GetComponent<IDamageable>();

            if(target != null)
            {
                target.OnDamage(damage, hit.point, hit.normal);
            }
            else
            {
                EffectManager.Instance.PlayHitEffect(hit.point, hit.normal);
            }

            hitPosition = hit.point;
        }
        else
        {
            hitPosition = firePosition.position + firePosition.forward * fireDistance;
        }

        StartCoroutine(ShotEffect(hitPosition));
        magAmmo--;
        if(magAmmo <= 0)
        {
            state = State.Empty;
        }
    }

    public bool Reload()
    {
        //������ ���� X ����(����, ź��)
        if(state == State.Reloading || magAmmo == magCapacity) return false;
        StartCoroutine("ReloadRoutine");
        return true;
    }
}
