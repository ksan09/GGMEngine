using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAction : MonoBehaviour
{
    public static CameraAction instance { get; private set; }

    private CinemachineVirtualCamera _vCam;
    private CinemachineBasicMultiChannelPerlin _vCamPerlin;

    private float totalTime = 0f;
    private float crtTime = 0f;
    private float startIntensity;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        _vCam = GetComponent<CinemachineVirtualCamera>();
        _vCamPerlin = _vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        if(crtTime > 0)
        {
            crtTime -= Time.deltaTime;
            if (crtTime < 0) crtTime = 0;
            _vCamPerlin.m_AmplitudeGain = Mathf.Lerp(startIntensity, 0f, 1 - crtTime / totalTime);
        }
    }

    public void ShakeCam(float intensity, float time)
    {
        _vCamPerlin.m_AmplitudeGain = intensity;
        totalTime = crtTime = time;
        startIntensity = intensity;
    }

}
