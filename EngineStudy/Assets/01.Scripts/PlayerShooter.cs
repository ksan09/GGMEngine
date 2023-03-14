using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public Gun gun;
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }


    // Update is called once per frame
    void Update()
    {
        if (playerInput.Fire)
        {
            gun.Fire();
        }
        else if (playerInput.Reload)
        {
            gun.Reload();
            //������ �ִϸ��̼� ����
        }
    }
}
