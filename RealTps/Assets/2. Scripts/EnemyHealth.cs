using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : LivingEntity
{
    //�� �ǰ� �� �ǰ� ����Ʈ, ��� ó��
    //LivingEntity ��ӹ޾Ƽ� 
    public override void OnDamage(float damage, Vector3 hitPosition, Vector3 hitNormal)
    {
        //�ǰ� �� ü���� damage��ŭ ����ش�.
        base.OnDamage(damage, hitPosition, hitNormal);
        StartCoroutine(ShowBloodEffect(hitPosition, hitNormal));
    }

    private IEnumerator ShowBloodEffect(Vector3 hitPosition, Vector3 hitNormal)
    {
        EffectManager.Instance.PlayHitEffect(hitPosition, hitNormal, transform, EffectManager.EffectType.Flesh);
        yield return new WaitForSeconds(1.0f);
    }

    public override void Die()
    {
        base.Die();
        GetComponent<MonsterCtrl>().state = MonsterCtrl.State.DIE;
    }
}
