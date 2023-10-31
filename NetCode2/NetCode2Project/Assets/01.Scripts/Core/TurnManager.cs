using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class TurnManager : NetworkBehaviour
{
    public NetworkVariable<GameRole> currentTurn = new NetworkVariable<GameRole>();

    private void SwitchTurn()
    {
        if(currentTurn.Value == GameRole.Client)
        {
            currentTurn.Value = GameRole.Host;
        }
        else
        {
            currentTurn.Value = GameRole.Client;
        }

        Debug.Log(currentTurn.Value);
    }

    public void StartGame()
    {
        currentTurn.Value = GameRole.Host;
        //SwitchTurn();
    }

    public override void OnNetworkSpawn()
    {
        GameManager.Instance.GameStateChanged += HandleGameStateChanged;
        Egg.OnHit += SwitchTurn;
    }

    public override void OnNetworkDespawn()
    {
        GameManager.Instance.GameStateChanged -= HandleGameStateChanged;
        Egg.OnHit -= SwitchTurn;
    }

    private void HandleGameStateChanged(GameState state)
    {
        if(state == GameState.Game)
        {
            StartGame();
        }
    }
}
