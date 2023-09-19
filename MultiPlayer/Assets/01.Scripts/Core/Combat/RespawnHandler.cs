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
        Destroy(player.gameObject);
        StartCoroutine(RespawnPlayer(player.OwnerClientId));
    }

    private IEnumerator RespawnPlayer(ulong ownerClientId)
    {
        yield return null;
        var instance = Instantiate(_playerPrefab, TankSpawnPoint.GerRandomSpawnPos(), Quaternion.identity);

        //�������� ���� �÷��̾ ��� Ŭ�󿡰� ����� ����
        // ���ÿ� �� �÷��� ���� ���������� �˷���
        instance.NetworkObject.SpawnAsPlayerObject(ownerClientId);
    }
}
