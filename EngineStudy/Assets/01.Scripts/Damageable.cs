using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Damageable
{
    void OnDamage(float damage, Vector3 hitPosition, Vector3 hitNormal); //피격량, 피격 위치, 피격 이펙트구현
}
