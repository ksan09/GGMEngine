using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public Gun gun;
    private PlayerInput playerInput;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();

        playerInput.OnFirePressed += FireButtonHandle;
    }


    // Update is called once per frame
    void Update()
    {
        if (playerInput.Reload)
        {
            gun.Reload();
            //재장전 애니메이션 실행
        }
    }

    private void FireButtonHandle()
    {
        playerMovement.SetRotation();
        gun.Fire();
    }
}
