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
    public Transform centerPoint;
    public LayerMask rayMask;
    public CapsuleCollider capCollider;
    public Transform followTransform;
    public Stats stats;

    [Header("Movement Settings")]    
    Vector3 moveDirection;
    float moveX;
    float moveZ;
    
    [Header("Control Settings")]
    public float sensitivity = 100f;
    Vector2 mousePos;
    

    [Header("Jump Settings")]
    public float maxVelocity;
    public float initialVelocity = 0;
    public float jumpTime = 0;
    public float jumpSpeed = 0;
    public float jumpHeight = 1;
    float jumpVelocity = 0;
    public float jumpDelay = 0;
    
    public bool isControlling;
    public bool isAttacking;



    private void OnEnable()
    {
        isControlling = true;

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

    void Update()
    {
        moveDirection = moveX * transform.forward + moveZ * transform.right;
        rb.velocity = new Vector3(moveDirection.x * stats.speed, rb.velocity.y, moveDirection.z * stats.speed);
    }

    void FixedUpdate()
    {
        if (isControlling)
        {
            float x = playerInputs.Player.Mouse.ReadValue<Vector2>().x;
            float y = -playerInputs.Player.Mouse.ReadValue<Vector2>().y;

            //transform.Rotate(Vector3.up * x * Time.deltaTime * sensitivity);

         
           followTransform.rotation *= Quaternion.AngleAxis(x * sensitivity, Vector3.up);
           followTransform.rotation *= Quaternion.AngleAxis(y * sensitivity, Vector3.right);
            
            
            

            var angles = followTransform.eulerAngles;
            angles.z = 0;
            var angle = followTransform.eulerAngles.x;

            if(angle > 180 && angle < 340)
            {
                angles.x = 340;
            }
            else if(angle < 180  && angle > 40)
            {
                angles.x = 40;
            }

            followTransform.eulerAngles = angles;          
            if (moveX == 0 && moveZ == 0)
            {
                return;
            }

            transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, transform.eulerAngles.y, 0), Quaternion.Euler(0, followTransform.eulerAngles.y, 0), 0.2f);
            followTransform.eulerAngles = new Vector3(angles.x, angles.y, 0);              
        }

        if (jump)
        {
            CalculateJump();
            if (gameObject.GetComponent<Rigidbody>().velocity.y < -0.1)
            {
                animator.SetBool("Falling", true);
            }
        }
    }

    

    #region INPUT CALLBACKS
    public void OnSpawnItem()
    {
        ItemSystem.SpawnRandom(transform.position + Vector3.up * 5);
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
    public void OnAttackRight()
    {
        animator.SetTrigger("R_Attack");
    }
    public void OnAttackLeft()
    {
        animator.SetTrigger("L_Attack");
    }
    #endregion

    #region RUN FUNCTION
    bool run = false;
    public void OnRun()
    {
        #region Animations
        if (!run)
        {
            stats.speed *= 1.60f;
            animator.SetBool("Run", true);
            run = true;
        }
        else if (run)
        {
            stats.speed /= 1.60f;
            animator.SetBool("Run", false);
            run = false;
        }      
        #endregion
    }
    #endregion

    #region JUMP FUNCTION
    bool jump = false;  
    public void OnJump()
    {
        if (CheckGrounded())
        {
            StartCoroutine(Jump());
            StartCoroutine(StatusEffects.HastenTarget(gameObject, 0.50f, 0.5f));
            jumpTime = initialVelocity;
            #region Animations
            animator.SetTrigger("Jump");           
            #endregion
        }
    }
    

    public IEnumerator Jump()
    {       
        yield return new WaitForSeconds(jumpDelay);
        jump = true;
        yield return new WaitWhile(CheckGrounded);
        yield return new WaitUntil(CheckGrounded);
        animator.SetBool("Falling", false);
        jump = false;
    }
    


    public void CalculateJump()
    {
        jumpTime += Time.deltaTime * jumpSpeed;
        if (jumpTime < ((3 * Mathf.PI) / 2) - maxVelocity && !Physics.Raycast(centerPoint.position, Vector3.up, capCollider.height/2 + 0.2f, rayMask))
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
    #endregion

    public bool CheckGrounded()
    {
        if (Physics.Raycast(centerPoint.position, Vector3.down, capCollider.height/2 + 0.2f, rayMask))
        {
            Debug.Log("Grounded");
            return true;
        }       
        else
        {
            Debug.Log("Not Grounded");
            return false;
        }
    }   

    private void OnDisable()
    {
        playerInputs.Player.Disable();
    }
}
