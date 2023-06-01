using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCaster : MonoBehaviour
{
    [SerializeField]
    private float _casterRadius = 1f;
    [SerializeField]
    private float _casterInterpolation = 0.5f;

    [SerializeField]
    private int _monsterDamage = 2;

    [SerializeField]
    private LayerMask _whatIsEnemy;

    public void DamageCast()
    {
        RaycastHit hit;
        bool isHit = Physics.SphereCast(transform.position - transform.forward * _casterRadius, _casterRadius, transform.forward, out hit, _casterRadius + _casterInterpolation, _whatIsEnemy);
        if(isHit)
        {
            Debug.Log(hit.collider.name);
            if(hit.collider.TryGetComponent<IDamageable>(out IDamageable target))
            {
                target.OnDamage(_monsterDamage, hit.point, hit.normal);
            }
        }else
        {
            Debug.Log("안맞았어요");
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _casterRadius);
        Gizmos.color = Color.white;
    }
#endif
}
