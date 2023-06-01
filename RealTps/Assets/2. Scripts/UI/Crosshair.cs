using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    public Image aimPointReticle;   //������ġ �̹���
    public Image hitPointReticle;   //���� �´� ��ġ �̹���

    public float smoothTime = 0.2f;  //�������� �̵��� �� �ε巴�� �̵��ϵ���

    private Camera screenCamera;     
    private RectTransform crosshairRectTransform;    //�ѾƸ� ���� �°ԵǴ� ��ġ

    private Vector2 currentHitPointVelocity;   //�������� ����� ���� ��ȭ��
    private Vector2 targetPoint; //ȭ��� ��ġ�� ��ȯ


    private void Awake()
    {
        screenCamera = Camera.main; //ī�޶� ����
        crosshairRectTransform = hitPointReticle.GetComponent<RectTransform>(); //���� ������ ��ġ ��������
    }

    public void SetActiveCrosshair(bool active) //������ Ȱ��ȭ, ��Ȱ��ȭ
    {
        hitPointReticle.enabled = active;
        aimPointReticle.enabled = active;
    }

    public void UpdatePosition(Vector3 worldPoint)
    {
        targetPoint = screenCamera.WorldToScreenPoint(worldPoint); //worldPoint�� WorldScreemPoint�� �ٲ��ֱ�
    }

    private void Update()
    {
        if (!hitPointReticle.enabled) return;
        crosshairRectTransform.position = Vector2.SmoothDamp(crosshairRectTransform.position, targetPoint, ref currentHitPointVelocity, smoothTime);
    }
}
