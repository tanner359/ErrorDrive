using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controller : MonoBehaviour
{
    [Header("References")]
    public Player_Inputs playerInputs;
    public Rigidbody rb;
    public Animator animator;
    BoxCollider headCollider;
    public LayerMask headCollisionFilter;

    [Header("Movement Settings")]
    public float movement_Speed = 1f;
    public Vector3 moveDirection;
    public float moveX;
    public float moveZ;
    
    [Header("Control Settings")]
    public Vector2 mousePos;
    public float sensitivity = 100f;

    [Header("Jump Settings")]
    public float maxVelocity;
    public float initialVelocity = 0;
    float jumpTime = 0;
    public float jumpSpeed = 0;
    public float jumpHeight = 1;
    float jumpVelocity = 0;

    bool isControlling;

    public void SetControl(bool state)
    {
        isControlling = state;
    }

    private void OnEnable()
    {
        isControlling = true;

        if (playerInputs == null)
        {
            playerInputs = new Player_Inputs();
        }
        
        playerInputs.Player.Enable();
    }

    private void Awake()
    {
        headCollider = gameObject.GetComponent<BoxCollider>();
        
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;      
        
    }

    private void Update()
    {       
        if (isControlling)
        {
            float x = playerInputs.Player.Mouse.ReadValue<Vector2>().x;
            float y = playerInputs.Player.Mouse.ReadValue<Vector2>().y;

            transform.Rotate(Vector3.up * x * Time.deltaTime * sensitivity);
        }        
    }

    void FixedUpdate()
    {
        if (isControlling)
        {
            moveDirection = moveX * transform.forward + moveZ * transform.right;
            rb.velocity = moveDirection * movement_Speed;
        }

        if (!grounded)
        {
            Jump();
        }
    }

    public void OnMovement(InputValue value)
    {
        if (isControlling)
        {
            moveX = (value.Get<Vector2>().y);
            moveZ = (value.Get<Vector2>().x);

            #region Animations
            if (moveX > 0)
            {
                animator.SetBool("Walk", true);
            }
            else
            {
                animator.SetBool("Walk", false);
            }
            if(moveX < 0)
            {
                animator.SetBool("WalkBackwards", true);
            }
            else
            {
                animator.SetBool("WalkBackwards", false);
            }
            if (moveZ > 0)
            {
                animator.SetBool("StrafeRight", true);
            }
            else
            {
                animator.SetBool("StrafeRight", false);
            }
            if(moveZ < 0)
            {
                animator.SetBool("StrafeLeft", true);
            }
            else
            {
                //rb.velocity = Vector3.zero;
                animator.SetBool("StrafeLeft", false);              
            }
            #endregion
        }
    }

    bool run = false;
    public void OnRun()
    {
        #region Animations
        if (!run)
        {
            movement_Speed = movement_Speed * 1.60f;
            animator.SetBool("Run", true);
            run = true;
        }
        else if (run)
        {
            movement_Speed = movement_Speed / 1.60f;
            animator.SetBool("Run", false);
            run = false;
        }      
        #endregion
    }


    bool grounded = true;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            grounded = true;
        }
    }

    public void OnJump()
    {       
        if (grounded)
        {
            grounded = false;
            jumpTime = initialVelocity;
            #region Animations
            animator.SetTrigger("Jump");
            //if (gameObject.GetComponent<Rigidbody>().velocity.y < 0)
            //{ // fall
            //    animator.SetBool("Falling", true);
            //}
            #endregion
        }
    }

    public void Jump()
    {
        Debug.Log("Jump");
        List<Collider> headCollisions = new List<Collider>();
        jumpTime += Time.deltaTime * jumpSpeed;
        if (jumpTime < ((3 * Mathf.PI) / 2) - maxVelocity && Physics.OverlapBox(headCollider.transform.position, headCollider.size, Quaternion.identity, headCollisionFilter).Length == 0)
        {
            jumpVelocity = Mathf.Sin(0.9f * jumpTime) * jumpHeight;
        }
        else
        {
            jumpTime = ((3 * Mathf.PI) / 2) - maxVelocity;
            jumpVelocity = Mathf.Sin(0.9f * jumpTime) * jumpHeight;
        }
        rb.velocity = new Vector3(rb.velocity.x, jumpVelocity, rb.velocity.z);
    }

    private void OnDisable()
    {
        playerInputs.Player.Disable();
    }
}
