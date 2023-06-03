using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField]
    private VisualEffect _bloodEffect;
    [SerializeField]
    private LayerMask _whatIsGround;
    private readonly int _heightHash = Shader.PropertyToID("boxHeight");

    [SerializeField]
    private int _maxHP;
    private int _currentHP;
       

    public UnityEvent OnDeadTriggered;

    private void Start()
    {
        _currentHP = _maxHP;
    }

    public void OnDamage(int damage, Vector3 point, Vector3 normal)
    {
        _currentHP -= damage;

        RaycastHit hit;
        if(Physics.Raycast(point, Vector3.down, out hit, 10f, _whatIsGround))
        {
            Quaternion look = Quaternion.LookRotation(normal);
            VisualEffect effect = Instantiate(_bloodEffect, point, look);

            effect.SetFloat(_heightHash, -hit.distance);
            effect.Play();
            Destroy(effect.gameObject, 3f);
        }

        _currentHP = Mathf.Clamp(_currentHP, 0, _maxHP);
        if(_currentHP <= 0)
        {
            OnDeadTriggered?.Invoke();
        }
    }
}
