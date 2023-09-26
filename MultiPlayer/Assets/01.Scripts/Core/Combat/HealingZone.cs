using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Netcode;
using UnityEngine;

public class HealingZone : NetworkBehaviour
{
    [Header("������")]
    [SerializeField] private Transform _healPowerBarTrm;
    [SerializeField] private Transform _chargePowerBarTrm;

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
        if (!IsServer) return;

        if (_remainCooldown > 0)
        {
            _remainCooldown -= Time.deltaTime;
            ChargePowerClientRPC(1-(_remainCooldown / _cooldown));
            if(_remainCooldown < 0)
            {
                ChargePowerClientRPC(0); // ���߿� �ڷ�ƾ ����?
                _healPower.Value = _maxHealPower;
            }
            else
            {
                return;
            }
        }

        // ���� �Դٶ�� �� �� �Ŀ��� �ִ�.
        _tickTimer += Time.deltaTime;
        if(_tickTimer >= _healTickRate)
        {
            //�ƹ�ư ���� �������� �����ΰ��� ���ְ�
            foreach(var player in _playersInZone)
            {
                if (player.HealthCompo.currentHealth.Value == 
                    player.HealthCompo.MaxHealth)
                    continue;
                if (player.Coin.totalCoins.Value < _coinPerTick)
                    continue;

                player.Coin.SpendCoin(_coinPerTick);
                player.HealthCompo.RestoreHealth(_healPerTick);
                _healPower.Value -= 1;
                if (_healPower.Value <= 0)
                {
                    _remainCooldown = _cooldown;
                    break;
                }
            }


            _tickTimer = _tickTimer % _healTickRate;
        }
    }

    [ClientRpc]
    public void ChargePowerClientRPC(float percent)
    {
        _chargePowerBarTrm.localScale = new Vector3(percent, 1, 1);
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
            .TryGetComponent<TankPlayer>(out TankPlayer player))
        {
            _playersInZone.Add(player);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!IsServer) return;

        if (col.attachedRigidbody
            .TryGetComponent<TankPlayer>(out TankPlayer player))
        {
            _playersInZone.Remove(player);
        }
    }

}
