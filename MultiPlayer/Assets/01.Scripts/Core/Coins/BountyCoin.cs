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
            SetVisible(false);  // 뽀개는 건 서버가 할 거니까
            // 클라는 그냥 안 보이게만 한다.
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
                //시퀀스가 끝나고 할 일
                _impulseSource.GenerateImpulse(0.3f);
            });

        
    }
}
