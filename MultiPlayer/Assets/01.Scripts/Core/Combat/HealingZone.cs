using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Netcode;
using UnityEngine;

public class HealingZone : NetworkBehaviour
{
    [Header("참조값")]
    [SerializeField] private Transform _healPowerBarTrm;
    [SerializeField] private Transform _chargePowerBarTrm;

    [Header("셋팅값")]
    [SerializeField] private int _maxHealPower = 30;    // 회복시킬 수 있는 틱수
    [SerializeField] private float _cooldown = 60f;     // 1분 쿨다운
    [SerializeField] private float _healTickRate = 1f;  // 힐 틱시간
    [SerializeField] private int _coinPerTick = 5;      // 1틱당 소모할 코인양
    [SerializeField] private int _healPerTick = 10;     // 1틱에 채워질 체력량

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
                ChargePowerClientRPC(0); // 나중에 코루틴 수정?
                _healPower.Value = _maxHealPower;
            }
            else
            {
                return;
            }
        }

        // 여기 왔다라는 건 힐 파워가 있다.
        _tickTimer += Time.deltaTime;
        if(_tickTimer >= _healTickRate)
        {
            //아무튼 뭔가 힐이차는 무엇인가를 해주고
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
