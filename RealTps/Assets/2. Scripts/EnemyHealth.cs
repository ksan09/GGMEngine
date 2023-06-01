using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : LivingEntity
{
    //적 피격 시 피격 이펙트, 사망 처리
    //LivingEntity 상속받아서 
    public override void OnDamage(float damage, Vector3 hitPosition, Vector3 hitNormal)
    {
        //피격 시 체력을 damage만큼 깎아준다.
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
