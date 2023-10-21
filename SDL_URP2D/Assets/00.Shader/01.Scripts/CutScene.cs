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

        StopAllCoroutines();
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
        currentTime = 0;
        float delayTime = 0.8f;
        yield return new WaitForSeconds(delayTime);
        currentTime = 0;
        float animationTime1 = 0.1f;
        while (currentTime < animationTime1)
        {
            _mat.SetVector(circleSizeVecName,
                Vector3.Lerp(endSize, 
                endSize + Vector3.one * 20, 
                currentTime / animationTime1));

            yield return null;
            currentTime += Time.deltaTime;
        }
        currentTime = 0;
        float animationTime2 = 0.1f;
        while (currentTime < animationTime2)
        {
            _mat.SetVector(circleSizeVecName,
                Vector3.Lerp(endSize + Vector3.one * 20, 
                Vector3.zero, 
                currentTime / animationTime2));

            yield return null;
            currentTime += Time.deltaTime;
        }
        _mat.SetVector(circleSizeVecName, Vector3.zero);
    }
}
