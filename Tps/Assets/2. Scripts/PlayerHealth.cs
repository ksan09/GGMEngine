using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : LivingEntity
{
    private int life = 3;

    public override void OnDamage(float damage, Vector3 hitPos, Vector3 hitNormal)
    {
        base.OnDamage(damage, hitPos, hitNormal);
        StartCoroutine(ShowBloodEffect(hitPos, hitNormal));

        UpdateUI();
    }

    private IEnumerator ShowBloodEffect(Vector3 hitPos, Vector3 hitNormal)
    {
        EffectManager.Instance.PlayHitEffect(hitPos, hitNormal, transform, EffectManager.EffectType.Flesh);
        CameraAction.instance.ShakeCam(4, 0.2f);
        yield return new WaitForSeconds(1.0f);
    }

    public override void RestoreHealth(float newHealth)
    {
        if (dead) return;

        base.RestoreHealth(newHealth);
        UpdateUI();
    }

    public override void Die()
    {
        //ав╬Н
        UIManager.Instance.UpdateLifeText(life);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (dead) return;

        IItem item = other.GetComponent<IItem>();
        if(item != null)
        {
            item.Use(gameObject);
        }
    }

    private void UpdateUI()
    {
        UIManager.Instance.UpdateHealthText(dead ? 0f : health);
    }
}
