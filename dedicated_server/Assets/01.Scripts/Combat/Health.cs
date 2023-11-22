using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class Health : NetworkBehaviour
{
    public int maxHealth = 100;
    public NetworkVariable<int> currentHealth;

    private bool _isDead = false;

    public Action<Health> OnDie;
    public UnityEvent<int, int, float> OnHealthChanged;

    // ���� ó��
    // currentHealth�� ���ؼ� ������ Ŭ�� �ٸ��� �ؾ���

    private void Awake()
    {
        currentHealth.Value = maxHealth;
        currentHealth.OnValueChanged += HandleChangeHealth;
    }

    private void HandleChangeHealth(int prev, int newValue)
    {
        // ���ߵ�
        OnHealthChanged?.Invoke(prev, newValue, (float)newValue / maxHealth);
    }

    public void TakeDamage(int damageValue)
    {
        ModifyHealth(-damageValue);
    }

    public void RestoreHealth(int healValue)
    {
        ModifyHealth(healValue);
    }

    public void ModifyHealth(int value)
    {
        if (_isDead) return;

        currentHealth.Value = currentHealth.Value + value;
        if (currentHealth.Value <= 0)
            _isDead = true;
    }
}
