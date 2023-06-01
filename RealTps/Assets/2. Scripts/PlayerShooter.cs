using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerMovement playerMovement;
    public Gun gun;
    private Animator animator;

    private Vector3 aimPoint;
    private Camera playerCamera;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        playerCamera = Camera.main;
    }

    private void UpdateAimTarget()
    {
        RaycastHit hit;
        var ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if(Physics.Raycast(ray, out hit, gun.fireDistance))
        {
            aimPoint = hit.point;
        }
        else
        {
            aimPoint = playerCamera.transform.position + playerCamera.transform.forward * gun.fireDistance;
        }
    }


    private void Update()
    {
        UpdateAimTarget();

        if (playerInput.fire)
        {
            gun.Fire(aimPoint);
        }
        else if (playerInput.reload)
        {
            if (gun.Reload()) animator.SetTrigger("Reload");
        }
    }

    private void UpdateUI()
    {
        if (gun == null || UIManager.Instance == null) return;
      
    }


}
