using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour, IItem
{
    public float newHealth = 50; //회복수치

    public void Use(GameObject target)
    {
        LivingEntity health = target.GetComponent<LivingEntity>();
        
        if(health != null)
        {
            health.RestoreHealth(newHealth);
        }

        Destroy(gameObject);
    }

}
