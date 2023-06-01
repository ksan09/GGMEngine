using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3f, rotationSpeed = 5f, gravity = -9.8f;
    public float rotationSmoothVelocity = 1f;

    [HideInInspector] public Vector3 dir;
    CharacterController characterController;
    PlayerInput playerInput;
    public GameObject followCamPos;


    private Animator animator;
    private Camera followCam;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        followCam = Camera.main;
    }

    private void Update()
    {
      //  transform.rotation *= Quaternion.AngleAxis(*rotationSpeed, Vector3.up); 
    }


    // Update is called once per frame
    void LateUpdate()
    {
        PlayerMove();
        PlayerRotate();
        UpdateAnimation(playerInput.moveDir);
    }


    void PlayerMove()
    {
        dir = transform.forward * playerInput.moveDir.y + transform.right * playerInput.moveDir.x;
        characterController.Move(dir * moveSpeed * Time.deltaTime);
    }


    void PlayerRotate()
    {
        float mouseX = playerInput.mouseX;
        float mouseY = playerInput.mouseY;

        mouseY = Mathf.Clamp(mouseY, -60f, 60f);
        var targetRotation = followCam.transform.eulerAngles.y;
        transform.eulerAngles = Vector3.up * targetRotation;
        //transform.eulerAngles = new Vector3(0, mouseX, 0);
        followCamPos.transform.eulerAngles = new Vector3(-mouseY, mouseX, 0);
    }

    private void UpdateAnimation(Vector2 moveInput)
    {
        animator.SetFloat("Vertical Move", moveInput.y);
        animator.SetFloat("Horizontal Move", moveInput.x);
    }

}
