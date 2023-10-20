using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CutScene : MonoBehaviour
{
    [SerializeField]
    private Transform _player;
    [SerializeField]
    private Material _mat;

    [SerializeField]
    private Vector3 _resolution;
    [SerializeField]
    private Vector3 _circleSize;

    private const string resolutionVecName  = "_Resolution";
    private const string circlePosVecName   = "_CirclePos";
    private const string circleSizeVecName  = "_CircleSize";

    private void Awake()
    {
        _mat.SetVector(resolutionVecName, _resolution);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            CutOffScene();
    }

    private void CutOffScene()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(_player.position) - (_resolution * 0.5f);
        _mat.SetVector(circlePosVecName, pos);

        StartCoroutine(CircleSizeChange(
            new Vector3(_resolution.x * 2, _resolution.x * 2), 
            _circleSize, 1f));
    }

    IEnumerator CircleSizeChange(Vector3 startSize, Vector3 endSize, float time)
    {
        float currentTime = 0;
        while(currentTime < time)
        {
            _mat.SetVector(circleSizeVecName,
                Vector3.Lerp(startSize, endSize, currentTime / time));

            yield return null;
            currentTime += Time.deltaTime;
        }
        _mat.SetVector(circleSizeVecName, endSize);
    }
}
