using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class ShootingSystem : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Transform _parent;

    [SerializeField] private Transform _gunNozzle;
    [SerializeField] private CinemachineFreeLook _cam;
    private CinemachineImpulseSource __impulseSource;

    private PlayerMovement _movement;

    private bool _isPressFire = false;

    private void Start()
    {
        _movement = GetComponent<PlayerMovement>();
        __impulseSource = _cam.GetComponent<CinemachineImpulseSource>();

        _inputReader.FireEvent += OnHandleFire;
    }

    private void OnHandleFire(bool value)
    {
        _isPressFire = value;
        

    }

    private void Update()
    {

        _movement.blockRotationPlayer = _isPressFire;

        if(_isPressFire)
        {
            //ÃÑÀ» ½ð´Ù
            VisualPolish();
        }

    }

    private void VisualPolish()
    {
        if(!DOTween.IsTweening(_parent))
        {
            _parent.DOComplete();
            Vector3 forward = _parent.forward;
            Vector3 localPos = _parent.localPosition;
            _parent.DOLocalMove(localPos-new Vector3(0, 0, 0.2f), 0.03f)
                .OnComplete(()=> _parent.DOLocalMove(localPos, 0.1f))
                .SetEase(Ease.OutSine);

            __impulseSource.GenerateImpulse(0.2f);
        }

        if (!DOTween.IsTweening(_gunNozzle))
        {
            _gunNozzle.DOComplete();
            _gunNozzle.DOPunchScale(new Vector3(0, 1, 1) / 1.5f, 0.15f, 10, 1);
        }
    }
}
