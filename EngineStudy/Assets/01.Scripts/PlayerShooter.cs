using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public Gun gun;
    private PlayerInput playerInput;
    private PlayerMovement playerMovement;
    private Animator animator;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();

        playerInput.OnFirePressed += FireButtonHandle;
    }


    // Update is called once per frame
    void Update()
    {
        if (playerInput.Reload)
        {
            if(gun.Reload())
            {
                animator.SetTrigger("Reload");
            }
            //������ �ִϸ��̼� ����
        }
    }

    private void FireButtonHandle()
    {
        playerMovement.SetRotation();
        gun.Fire();
    }
}
