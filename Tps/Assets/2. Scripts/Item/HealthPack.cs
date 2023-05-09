using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour, IItem
{
    public float newHealth = 100f;

    public void Use(GameObject target)
    {
        LivingEntity entity = target.GetComponent<LivingEntity>();
        if(entity != null)
        {
            entity.RestoreHealth(newHealth);
        }
        Destroy(gameObject);
    }
}
