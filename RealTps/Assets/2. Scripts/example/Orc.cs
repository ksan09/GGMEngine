using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : BaseMonster
{
    public override void Attack()
    {
        Debug.Log("���� ������ �ߴ�! ���� �Լ�! ������: " + damage);
    }
}
