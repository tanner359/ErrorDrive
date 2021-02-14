using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Player_Inputs playerInputs;
    public Rigidbody rb;
    public Vector2 moveSpeed;
    public Animator animator;

    private void OnEnable()
    {
        if (playerInputs == null)
        {
            playerInputs = new Player_Inputs();
        }

        playerInputs.Player.Enable();
    }

    private void Update()
    {
        
    }

    void FixedUpdate()
    {
        rb.velocity =  rb.velocity + new Vector3(moveSpeed.x, 0 , moveSpeed.y);        
    }


    public void OnMovement(InputValue value)
    {
        moveDirection = new Vector2(value.Get<Vector2>().x, value.Get<Vector2>().y);

        if (moveSpeed.y != 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }



    private void OnDisable()
    {
        playerInputs.Player.Disable();
    }
}
