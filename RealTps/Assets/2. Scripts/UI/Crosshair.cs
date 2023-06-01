using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    public Image aimPointReticle;   //조준위치 이미지
    public Image hitPointReticle;   //실제 맞는 위치 이미지

    public float smoothTime = 0.2f;  //조준점이 이동할 때 부드럽게 이동하도록

    private Camera screenCamera;     
    private RectTransform crosshairRectTransform;    //총아링 실제 맞게되는 위치

    private Vector2 currentHitPointVelocity;   //스무딩에 사용할 값의 변화량
    private Vector2 targetPoint; //화면상 위치로 변환


    private void Awake()
    {
        screenCamera = Camera.main; //카메라 연결
        crosshairRectTransform = hitPointReticle.GetComponent<RectTransform>(); //현재 조준점 위치 가져오기
    }

    public void SetActiveCrosshair(bool active) //조준점 활성화, 비활성화
    {
        hitPointReticle.enabled = active;
        aimPointReticle.enabled = active;
    }

    public void UpdatePosition(Vector3 worldPoint)
    {
        targetPoint = screenCamera.WorldToScreenPoint(worldPoint); //worldPoint를 WorldScreemPoint로 바꿔주기
    }

    private void Update()
    {
        if (!hitPointReticle.enabled) return;
        crosshairRectTransform.position = Vector2.SmoothDamp(crosshairRectTransform.position, targetPoint, ref currentHitPointVelocity, smoothTime);
    }
}
