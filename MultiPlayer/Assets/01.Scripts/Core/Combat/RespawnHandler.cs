using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RespawnHandler : NetworkBehaviour
{
    [SerializeField] private TankPlayer _playerPrefab;

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
        Destroy(player.gameObject);
        StartCoroutine(RespawnPlayer(player.OwnerClientId));
    }

    private IEnumerator RespawnPlayer(ulong ownerClientId)
    {
        yield return null;
        var instance = Instantiate(_playerPrefab, TankSpawnPoint.GerRandomSpawnPos(), Quaternion.identity);

        //서버에서 만든 플레이어를 모든 클라에게 만들라 전달
        // 동시에 이 플레가 누구 소유인지도 알려줌
        instance.NetworkObject.SpawnAsPlayerObject(ownerClientId);
    }
}
