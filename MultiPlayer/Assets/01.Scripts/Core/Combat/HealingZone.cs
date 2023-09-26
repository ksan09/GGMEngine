using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HealingZone : NetworkBehaviour
{
    [Header("������")]
    [SerializeField] private Transform _healPowerBarTrm;

    [Header("���ð�")]
    [SerializeField] private int _maxHealPower = 30;    // ȸ����ų �� �ִ� ƽ��
    [SerializeField] private float _cooldown = 60f;     // 1�� ��ٿ�
    [SerializeField] private float _healTickRate = 1f;  // �� ƽ�ð�
    [SerializeField] private int _coinPerTick = 5;      // 1ƽ�� �Ҹ��� ���ξ�
    [SerializeField] private int _healPerTick = 10;     // 1ƽ�� ä���� ü�·�

    private List<TankPlayer> _playersInZone = new List<TankPlayer>();

    private NetworkVariable<int> _healPower = new NetworkVariable<int>();

    private float _remainCooldown;
    private float _tickTimer;

    private void Update()
    {
        
    }

    public override void OnNetworkSpawn()
    {
        if(IsClient)
        {
            _healPower.OnValueChanged += HandleHealPowerChanged;
            HandleHealPowerChanged(0, _healPower.Value);
        }

        if(IsServer)
        {
            _healPower.Value = _maxHealPower;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsClient)
        {
            _healPower.OnValueChanged -= HandleHealPowerChanged;
        }
    }

    private void HandleHealPowerChanged(int oldPower, int newPower)
    {
        _healPowerBarTrm.localScale = new Vector3((float)newPower / _maxHealPower,
            1, 1);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!IsServer) return;

        if (col.attachedRigidbody
            .TryGetComponent<TankPlayer>(out TankPlayer temp))
        {
            _playersInZone.Add(temp);
            Debug.Log($"{temp.name} ����");
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!IsServer) return;

        if (col.attachedRigidbody
            .TryGetComponent<TankPlayer>(out TankPlayer temp))
        {
            _playersInZone.Remove(temp);
            Debug.Log($"{temp.name} ����");
        }
    }

}
