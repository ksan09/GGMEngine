using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RespawnHandler : NetworkBehaviour
{
    [SerializeField] private TankPlayer _playerPrefab;
    [SerializeField] private float _keptCoinRatio; // �׾ �����ϰ� ���� ���� ����

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        TankPlayer[] players = FindObjectsByType<TankPlayer>(FindObjectsSortMode.None);
        foreach(var p in players)
        {
            HandlePlayerSpawned(p); // �� ������Ʈ �̹� �����̶��
        }

        TankPlayer.OnPlayerSpawned += HandlePlayerSpawned;
        TankPlayer.OnPlayerDespawned += HandlePlayerDespawned;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsServer) return;
        TankPlayer.OnPlayerSpawned -= HandlePlayerSpawned;
        TankPlayer.OnPlayerDespawned -= HandlePlayerDespawned;
    }

    private void HandlePlayerDespawned(TankPlayer p)
    {
       p.HealthCompo.OnDie -= HandlePlayerDie;
    }

    private void HandlePlayerSpawned(TankPlayer p)
    {
        p.HealthCompo.OnDie += HandlePlayerDie;
    }

    private void HandlePlayerDie(Health player)
    { 
        int remainCoin = Mathf
            .FloorToInt(player.Tank.Coin.totalCoins.Value * _keptCoinRatio);
        if (player.Tank.Coin.totalCoins.Value <= 10)
        {
            remainCoin = 0;
        }


        Destroy(player.gameObject);
        StartCoroutine(RespawnPlayer(player.OwnerClientId, remainCoin));
    }

    private IEnumerator RespawnPlayer(ulong ownerClientId, int remainCoin)
    {
        yield return null;
        var instance = Instantiate(_playerPrefab, TankSpawnPoint.GerRandomSpawnPos(), Quaternion.identity);

        //�������� ���� �÷��̾ ��� Ŭ�󿡰� ����� ����
        // ���ÿ� �� �÷��� ���� ���������� �˷���
        instance.NetworkObject.SpawnAsPlayerObject(ownerClientId);
        instance.Coin.totalCoins.Value = remainCoin;
    }
}
