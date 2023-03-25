using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController; 
    private PlayerInput playerInput; 
    private Animator animator; 

    private Camera followCam; 

    public float targetSpeed = 6f;

   public float currentSpeed => new Vector2(characterController.velocity.x, characterController.velocity.z).magnitude;



    // Start is called before the first frame update
    void Start()
    {
        //컴포넌트 가져오기
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        followCam = Camera.main;
    }

    private void FixedUpdate() 
    {
        Move(playerInput.moveInput);
        Rotate();
    }


    // Update is called once per frame
    void Update()
    {
        UpdateAnimation(playerInput.moveInput);
    }

    public void Move(Vector2 moveInput)
    {
        var moveDir = Vector3.Normalize(transform.forward * moveInput.y + transform.right * moveInput.x);

        var velocity = moveDir * targetSpeed;

        characterController.Move(velocity * Time.deltaTime);

    }

    public void Rotate()
    {
        if (currentSpeed <= 0) return;

        var targetRotation = followCam.transform.eulerAngles.y;
        transform.eulerAngles = Vector3.up * targetRotation;
    }

    public void SetRotation()
    {
        Vector3 target = Vector3.zero;

        bool isHit = playerInput.GetMouseWorldPosition(out target);

        if(isHit)
        {
            Vector3 dir = target - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.LookRotation(dir.normalized);
        }
    }

     private void UpdateAnimation(Vector2 moveInput)
    {
        animator.SetFloat("Vertical Move", moveInput.y);
        animator.SetFloat("Horizontal Move", moveInput.x);
    }

}
