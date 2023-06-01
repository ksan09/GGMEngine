using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : BaseMonster
{
    public override void Attack()
    {
        Debug.Log("광역 공격을 했다! 전투 함성! 데미지: " + damage);
    }
}
