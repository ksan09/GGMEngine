using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraAction : MonoBehaviour
{
    public static CameraAction instance { get; private set; }
    private CinemachineVirtualCamera virtualCam;
    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;

    private float totalTime = 0;
    private float currentTime;
    private float startIntensity;

    private void Awake()
    {
        instance = this;
        Debug.Log(this.name);
        virtualCam = GetComponent<CinemachineVirtualCamera>();
        cinemachineBasicMultiChannelPerlin = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float intensity, float time)
    {
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        totalTime = currentTime = time;
        startIntensity = intensity;
    }

    private void Update()
    {
        if(currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            if (currentTime < 0) currentTime = 0;
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(startIntensity, 0f, 1 - currentTime / totalTime);
        }
    } 
}

