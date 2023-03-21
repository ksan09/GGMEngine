using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public Gun gun;
    private PlayerInput1 playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput1>();
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
            //재장전 애니메이션 실행
        }
    }
}
