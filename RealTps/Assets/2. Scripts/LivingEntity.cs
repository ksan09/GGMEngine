using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


// 생명체로 동작할 게임 오브젝트들을 위한 뼈대 제공
// 체력, 데미지 받아들이기, 사망 기능, 사망 이벤트 제공
public class LivingEntity : MonoBehaviour, IDamageable
{ 
    public float initHealth = 100f; //시작 체력
    public float health { get; protected set; } //현재 체력
                                                //상속받은 객체에서도 사용이 가능할 수 있도록 protected  사용
    public bool dead { get; protected set; } //사망 상태
    public UnityEvent OnDeath; //사망 시 발동할 이벤트
                                 //Action : 연결된 여러 매서드나 작업들이 동시에 실행되도록 하는 용도로 많이 사용
                                 //event: 클래스 외부에서 사용하도록 하는 것이 이벤트 처리

    //자식 클래스에서 override가 가능하도록 virtual 형태로 선언함
    
    //생명체가 활성화될 때 상태를 리셋
    protected virtual void OnEnable() 
    {
        dead = false;
        health = initHealth;
    }

    //데미지 입는 기능
    public virtual void OnDamage(float damage, Vector3 hitPosition, Vector3 hitNormal)
    {
        //피격 시 체력을 damage만큼 깎아준다.
        health -= damage;
        Debug.Log(health + "체력");

        //아직 사망중이 아니고 체력이 0 이하가 되면 사망 처리(Die())를 한다.
        if (health <=0 && !dead)
        {
            Die();
        }
    }

    //체력 회복 기능
    // 피격 이펙트는 상속받은 객체가 알아서 실행하고 체력 감소 부분만 구현
    public virtual void RestoreHealth(float newHealth)
    {
        //사망한 경우 체력 회복 불가
        if (dead) return;
        
        health += newHealth; //체력만큼 회복 
    }

    //사망처리
    public virtual void Die()
    {
        OnDeath?.Invoke();//onDeath 이벤트에 등록된 메서드가 있다면 실행
        dead = true; //사망 상태 true로 변경
    }
   
}
 