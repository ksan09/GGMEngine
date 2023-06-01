using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMonster : MonoBehaviour
{
    //���̽����ʹ� �ǽð����� ������ ����. 

    public float damage = 100f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    public abstract void Attack(); //��ӹ޴� ���̽����Ϳ��� ����. ���⼭ ���� ���ص� ��.
}
