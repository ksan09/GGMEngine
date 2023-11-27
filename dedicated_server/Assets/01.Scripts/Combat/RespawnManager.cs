
using UnityEngine;
using Unity.Netcode;
using System;
using System.Collections;

public class RespawnManager : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        Player.OnPlayerDespawned += HandlePlayerDespawn;
    }


    public override void OnNetworkDespawn()
    {
        if (!IsServer) return;

        Player.OnPlayerDespawned -= HandlePlayerDespawn;
    }

    private void HandlePlayerDespawn(Player player)
    {
        //
        ulong lastHitDealerID = player.HealthCompo.LastHitDealerID;
        UserData data = ServerSingleton.Instance.NetServer.GetUserDataByClientID(lastHitDealerID);
        UserData pData = ServerSingleton.Instance.NetServer.GetUserDataByClientID(player.OwnerClientId);

        if(pData != null)
        {
            Debug.Log($"{pData.username} is dead by {data.username}[{lastHitDealerID}]");
        }
        
        StartCoroutine(DelayRespawn(player.OwnerClientId));
    }

    IEnumerator DelayRespawn(ulong clientID)
    {
        yield return new WaitForSeconds(3f);
        ServerSingleton.Instance.NetServer.RespawnPlayer(clientID);
    }
}
