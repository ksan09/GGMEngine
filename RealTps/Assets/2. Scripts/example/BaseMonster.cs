using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMonster : MonoBehaviour
{
    //베이스몬스터는 실시간으로 찍히지 않음. 

    public float damage = 100f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    public abstract void Attack(); //상속받는 베이스몬스터에서 구현. 여기서 구현 안해도 됨.
}
