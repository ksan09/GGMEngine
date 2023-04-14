using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : LivingEntity
{
    public override void OnDamage(float damage, Vector3 hitPos, Vector3 hitNormal)
    {
        base.OnDamage(damage, hitPos, hitNormal);
        StartCoroutine(ShowBloodEffect(hitPos, hitNormal));
    }

    private IEnumerator ShowBloodEffect(Vector3 hitPos, Vector3 hitNormal)
    {
        EffectManager.Instance.PlayHitEffect(hitPos, hitNormal, transform, EffectManager.EffectType.Flesh);
        yield return new WaitForSeconds(1.0f);
    }

    public override void Die()
    {
        base.Die();
    }

}
