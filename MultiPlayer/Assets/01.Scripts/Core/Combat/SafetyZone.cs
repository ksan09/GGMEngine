using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class SafetyZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody.TryGetComponent<TankPlayer>(out TankPlayer player))
        {
            if(player.NetworkObject.IsOwner)
            {
                // 여기서 상점 메시지를 띄우고
                Debug.Log($"Enter : {player.playerName.Value}");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.attachedRigidbody.TryGetComponent<TankPlayer>(out TankPlayer player))
        {
            if (player.NetworkObject.IsOwner)
            {
                // 여기서 상점 메시지를 껴주고
                Debug.Log($"Exit : {player.playerName.Value}");
            }
        }
    }

}
