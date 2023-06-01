using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


// ����ü�� ������ ���� ������Ʈ���� ���� ���� ����
// ü��, ������ �޾Ƶ��̱�, ��� ���, ��� �̺�Ʈ ����
public class LivingEntity : MonoBehaviour, IDamageable
{ 
    public float initHealth = 100f; //���� ü��
    public float health { get; protected set; } //���� ü��
                                                //��ӹ��� ��ü������ ����� ������ �� �ֵ��� protected  ���
    public bool dead { get; protected set; } //��� ����
    public UnityEvent OnDeath; //��� �� �ߵ��� �̺�Ʈ
                                 //Action : ����� ���� �ż��峪 �۾����� ���ÿ� ����ǵ��� �ϴ� �뵵�� ���� ���
                                 //event: Ŭ���� �ܺο��� ����ϵ��� �ϴ� ���� �̺�Ʈ ó��

    //�ڽ� Ŭ�������� override�� �����ϵ��� virtual ���·� ������
    
    //����ü�� Ȱ��ȭ�� �� ���¸� ����
    protected virtual void OnEnable() 
    {
        dead = false;
        health = initHealth;
    }

    //������ �Դ� ���
    public virtual void OnDamage(float damage, Vector3 hitPosition, Vector3 hitNormal)
    {
        //�ǰ� �� ü���� damage��ŭ ����ش�.
        health -= damage;
        Debug.Log(health + "ü��");

        //���� ������� �ƴϰ� ü���� 0 ���ϰ� �Ǹ� ��� ó��(Die())�� �Ѵ�.
        if (health <=0 && !dead)
        {
            Die();
        }
    }

    //ü�� ȸ�� ���
    // �ǰ� ����Ʈ�� ��ӹ��� ��ü�� �˾Ƽ� �����ϰ� ü�� ���� �κи� ����
    public virtual void RestoreHealth(float newHealth)
    {
        //����� ��� ü�� ȸ�� �Ұ�
        if (dead) return;
        
        health += newHealth; //ü�¸�ŭ ȸ�� 
    }

    //���ó��
    public virtual void Die()
    {
        OnDeath?.Invoke();//onDeath �̺�Ʈ�� ��ϵ� �޼��尡 �ִٸ� ����
        dead = true; //��� ���� true�� ����
    }
   
}
 