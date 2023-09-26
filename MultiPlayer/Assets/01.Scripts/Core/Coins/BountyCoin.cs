using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BountyCoin : Coin
{
    [SerializeField] private CinemachineImpulseSource _impulseSource;

    public override int Collect()
    {
        if(!IsServer)
        {
            SetVisible(false);  // �ǰ��� �� ������ �� �Ŵϱ�
            // Ŭ��� �׳� �� ���̰Ը� �Ѵ�.
            return 0;
        }

        if (_alreadyCollected) return 0;
        _alreadyCollected = true;
        Destroy(gameObject);
        return _coinValue;
    }

    public void SetCoinToVisible(float coinScale)
    {
        isActive.Value = true;
        CoinSpawnClientRPC(coinScale);
    }

    [ClientRpc]
    private void CoinSpawnClientRPC(float coinScale)
    {
        Sequence seq = DOTween.Sequence();
        Vector3 dest = transform.position;
        transform.position = transform.position + new Vector3(0, 3f, 0);
        transform.localScale = Vector3.one * 0.5f;

        SetVisible(true);
        seq.Append(transform.DOScale(coinScale, 0.8f))
            .Join(transform.DOMove(dest, 0.8f).SetEase(Ease.OutBounce))
            .InsertCallback(0.6f, () =>
            {
                //�������� ������ �� ��
                _impulseSource.GenerateImpulse(0.3f);
            });

        
    }
}
