using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RespawnHandler : NetworkBehaviour
{
    [SerializeField] private TankPlayer _playerPrefab;
    [SerializeField] private float _keptCoinRatio; // 죽어도 보유하고 있을 코인 비율

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        TankPlayer[] players = FindObjectsByType<TankPlayer>(FindObjectsSortMode.None);
        foreach(var p in players)
        {
            HandlePlayerSpawned(p); // 이 오브젝트 이미 생성이라면
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

        //서버에서 만든 플레이어를 모든 클라에게 만들라 전달
        // 동시에 이 플레가 누구 소유인지도 알려줌
        instance.NetworkObject.SpawnAsPlayerObject(ownerClientId);
        instance.Coin.totalCoins.Value = remainCoin;
    }
}
