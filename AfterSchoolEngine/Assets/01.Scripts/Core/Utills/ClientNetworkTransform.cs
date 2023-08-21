using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;

public class ClientNetworkTransform : NetworkTransform
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        CanCommitToTransform = IsOwner;
    }

    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }

    protected override void Update()
    {
        CanCommitToTransform = IsOwner;
        base.Update();

        // ��Ʈ��ũ �Ŵ��� ����
        if(NetworkManager != null)
        {
            // ���� �Ǵ� ��������
            if(NetworkManager.IsConnectedClient || NetworkManager.IsListening)
            {
                // ���� ���ɽ�
                if(CanCommitToTransform)
                {
                    TryCommitTransformToServer(transform, NetworkManager.LocalTime.Time);
                }
            }
        }
        
    }
}
