using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
   


    public override void OnDamage(float damage, Vector3 hitPosition, Vector3 hitNormal)
    {
        base.OnDamage(damage, hitPosition, hitNormal);
        CameraAction.instance.ShakeCamera(4, 0.2f);
        StartCoroutine(ShowBloodEffect(hitPosition, hitNormal));

        UpdateUI();
    }

    private IEnumerator ShowBloodEffect(Vector3 hitPosition, Vector3 hitNormal)
    {
        EffectManager.Instance.PlayHitEffect(hitPosition, hitNormal, transform, EffectManager.EffectType.Flesh);
        yield return new WaitForSeconds(1.0f);
    }
  
    public override void Die()
    {
        //ÇÃ·¹ÀÌ¾î°¡ Á×À¸¸é? 
        base.Die();
     }
    
    private void UpdateUI()
    {
        UIManager.Instance.UpdateHealthText(dead ? 0f : health);
    }


    public override void RestoreHealth(float newHealth)
    {
        base.RestoreHealth(newHealth);
        UpdateUI();
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (!dead)
        {
            IItem item = other.GetComponent<IItem>();

            if(item != null)
            {
                item.Use(gameObject);
                Debug.Log("¾ÆÀÌÅÛ È¹µæ");
            }
        }
    }

}
