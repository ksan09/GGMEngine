using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DecalCircle : MonoBehaviour
{
    [Header("참조 변수")]
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public bool showDecal = false;

    public void OpenCircle(Vector3 point, float radius)
    {
        _spriteRenderer.color = new Color(1, 1, 1, 0);
        transform.position = point;
        transform.localScale = Vector3.zero;

        showDecal = true;
        Sequence seq = DOTween.Sequence();
        seq.Append(_spriteRenderer.DOFade(1, 0.3f));
        seq.Append(transform.DOScale(Vector3.one * (radius * 2), 0.8f));
    }

    public void CloseCircle()
    {
        showDecal = false;
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(Vector3.zero, 0.8f))
            .Join(_spriteRenderer.DOFade(0, 0.8f));
    }
}
