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

    // 구독 처리
    // currentHealth에 대해서 서버와 클라가 다르게 해야함

    private void Awake()
    {
        currentHealth.Value = maxHealth;
        currentHealth.OnValueChanged += HandleChangeHealth;
    }

    private void HandleChangeHealth(int prev, int newValue)
    {
        // 알잘딱
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
