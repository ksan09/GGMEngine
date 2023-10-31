using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class ScoreManager : NetworkBehaviour
{
    public NetworkVariable<int> hostScore = new NetworkVariable<int>();
    public NetworkVariable<int> clientScore = new NetworkVariable<int>();

    //
    private void HandleScoreChanged(int oldScore, int newScore)
    {
        SignalHub.OnScoreChanged(hostScore.Value, clientScore.Value);
    }
    //

    public override void OnNetworkSpawn()
    {
        hostScore.OnValueChanged      += HandleScoreChanged;
        clientScore.OnValueChanged    += HandleScoreChanged;

        if(!IsServer) return;
        Egg.OnFallInWater += HandleFallWater;
    }

    public override void OnNetworkDespawn()
    {
        hostScore.OnValueChanged    -= HandleScoreChanged;
        clientScore.OnValueChanged  -= HandleScoreChanged;

        if(!IsServer) return;
        Egg.OnFallInWater -= HandleFallWater;
    }

    private void HandleFallWater()
    {
        switch (GameManager.Instance.TurnManager.currentTurn.Value)
        {
            case GameRole.Host:
                clientScore.Value += 1;
                break;
            case GameRole.Client:
                hostScore.Value += 1;
                break;
            default:
                break;
        }

        CheckForEndGame();
    }

    // ���� �������� üũ ( �̰� ������ ������ ���� )
    private void CheckForEndGame()
    {
        //3�� ����
        // hostScore >= 3, GameManager RPC call host's win
        // clientScore >= 3, GameManager RPC call client's win
        if (hostScore.Value >= 3)
        {
            GameManager.Instance.SendResultToClient(GameRole.Host);
        }
        else if(clientScore.Value >= 3)
        {
            GameManager.Instance.SendResultToClient(GameRole.Client);
        }
        else
        {
            GameManager.Instance.EggManager.ResetEgg();
        }
        
        
    }

    private void Start()
    {
        InitializeScore();
    }

    public void InitializeScore()
    {
        hostScore.Value = 0;
        clientScore.Value = 0;
        //���߿� UI ���ű��� ���ٲ���
    }
}
