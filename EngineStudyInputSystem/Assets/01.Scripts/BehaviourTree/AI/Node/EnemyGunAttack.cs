using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGunAttack : EnemyAttack
{
    [SerializeField]
    private Gun _gun;

    public override void Attack()
    {
        _gun.Fire();
    }
    
    public bool NeedReloading()
    {
        return _gun.needReload;
    }

    public void Reloading()
    {
        _gun.Reload();
    }
}
