using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Player_Inputs playerInputs;
    public Rigidbody rb;
    public float moveSpeed = 1f;
    public Vector3 moveDirection;
    public float moveX;
    public float moveZ;
    public Animator animator;
    public Vector2 mousePos;
    public float sensitivity = 100f;

    private void OnEnable()
    {
        if (playerInputs == null)
        {
            playerInputs = new Player_Inputs();
        }

        playerInputs.Player.Enable();
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;      
    }

    private void Update()
    {
        float x = playerInputs.Player.Mouse.ReadValue<Vector2>().x;
        float y = playerInputs.Player.Mouse.ReadValue<Vector2>().y;

        Debug.Log(x);
        transform.Rotate(Vector3.up * x);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawIcon(mousePos, "Mouse", true, Color.red);
    //}


    void FixedUpdate()
    {        
        moveDirection = moveX * transform.forward + moveZ * transform.right;
        rb.velocity = moveDirection * moveSpeed;     
    }


    

    public void OnMovement(InputValue value)
    {
        moveX = (value.Get<Vector2>().y);
        moveZ = (value.Get<Vector2>().x);


        if (moveX != 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            rb.velocity = Vector3.zero;
            animator.SetBool("isWalking", false);
        }
    }



    private void OnDisable()
    {
        playerInputs.Player.Disable();
    }
}
