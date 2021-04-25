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
    public bool isControlling;
    public bool isAttacking;
    Vector2 mousePos;
    

    [Header("Jump Settings")]
    public float maxVelocity;
    public float initialVelocity = 0;
    public float jumpTime = 0;
    public float jumpSpeed = 0;
    public float jumpHeight = 1;
    float jumpVelocity = 0;
    public float jumpDelay = 0;

    [Header("Aiming")]
    public bool aimRight;
    public bool aimLeft;
    public Transform rightArmCTRL;
    public Transform leftArmCTRL;
    public InverseKinematics rightArmIK;
    public InverseKinematics leftArmIK;

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
    void FixedUpdate()
    {
        if (isControlling)
        {
            if (jump)
            {
                CalculateJump();
                if (gameObject.GetComponent<Rigidbody>().velocity.y < -0.1)
                {
                    animator.SetBool("Falling", true);
                }
            }
            moveDirection = moveX * transform.forward + moveZ * transform.right;
            rb.velocity = new Vector3(moveDirection.x * stats.speed, rb.velocity.y, moveDirection.z * stats.speed);
            CameraControl();
        }
    }
    private void LateUpdate()
    {
        if (aimLeft && aimRight)
        {
            leftArmCTRL.position = Reticle.instance.transform.position;
            rightArmCTRL.position = Reticle.instance.transform.position;
        }
        else if (aimRight)
        {
            rightArmCTRL.position = Reticle.instance.transform.position;
        }
        else if (aimLeft)
        {
            leftArmCTRL.position = Reticle.instance.transform.position;
        }      
    }

    #region INPUT CALLBACKS
    public void OnSpawnItem()
    {
        ItemSystem.SpawnRandom(transform.position + Vector3.up * 5);
    } 
    public void OnAim()
    {
        if (!CameraMaster.instance.aimCam.activeInHierarchy)
        {
            CameraMaster.instance.SwitchToCamera(CameraMaster.CMCams.aimCam);
        }
        else
        {
            CameraMaster.instance.SwitchToCamera(CameraMaster.CMCams.mainCam);
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
    public bool isShootingRight;
    public void OnAttackRight(InputValue value)
    {
        if (!isControlling){return;}

        float clickValue = value.Get<float>();
        Slot slot = InventorySystem.GetEquipSlot(Equip.Tags.Main_Hand).GetComponent<Slot>();

        if (clickValue == 1 && slot.item != null)
        {
            if (slot.item.itemClass == Item.ItemClass.Ranged && !isShootingRight)
            {
                StartCoroutine(ShootRight(slot));
                return;
            }
            else if (slot.item.itemClass == Item.ItemClass.Melee)
            {
                animator.SetTrigger("R_Attack");
                return;
            }
            return;
        }
        else if (clickValue == 0)
        {
            StartCoroutine(StopShootingRight());
        }
    }
    public bool isShootingLeft;
    public void OnAttackLeft(InputValue value)
    {
        if (!isControlling) { return; }

        float clickValue = value.Get<float>();
        Slot slot = InventorySystem.GetEquipSlot(Equip.Tags.Off_Hand).GetComponent<Slot>();

        if (clickValue == 1 && slot.item != null)
        {
            if (slot.item.itemClass == Item.ItemClass.Ranged && !isShootingLeft)
            {
                StartCoroutine(ShootLeft(slot));
                return;
            }
            else if (slot.item.itemClass == Item.ItemClass.Melee)
            {
                animator.SetTrigger("L_Attack");
                return;
            }
            return;
        }
        else if (clickValue == 0)
        {
            StartCoroutine(StopShootingLeft());         
        }
    }
    #endregion

    #region COROUTINES

    #region SHOOTING
    private IEnumerator StopShootingRight()
    {
        yield return new WaitWhile(() => isShootingRight);
        StopAllCoroutines();
    }
    private IEnumerator StopShootingLeft()
    {
        yield return new WaitWhile(() => isShootingLeft);
        StopAllCoroutines();
    }
    private IEnumerator ShootRight(Slot equipSlot)
    {
        float fireRate = equipSlot.item.fireRate;
        switch (equipSlot.item.firingMode)
        {
            case Item.FiringMode.auto:
                isShootingRight = true;
                Combat.FireBullet(equipSlot);
                yield return new WaitForSeconds(1 / fireRate);
                isShootingRight = false;
                yield return new WaitForSeconds(0.01f);
                StartCoroutine(ShootRight(equipSlot));
                break;

            case Item.FiringMode.burst:
                isShootingRight = true;
                Combat.FireBullet(equipSlot);
                yield return new WaitForSeconds(0.1f);
                Combat.FireBullet(equipSlot);
                yield return new WaitForSeconds(0.1f);
                Combat.FireBullet(equipSlot);
                yield return new WaitForSeconds(1 / fireRate);
                isShootingRight = false;
                yield return new WaitForSeconds(0.01f);
                StartCoroutine(ShootRight(equipSlot));
                break;

            case Item.FiringMode.semi:
                isShootingRight = true;
                Combat.FireBullet(equipSlot);
                yield return new WaitForSeconds(0.05f);
                isShootingRight = false;
                break;

            case Item.FiringMode.single:
                isShootingRight = true;
                Combat.FireBullet(equipSlot);
                yield return new WaitForSeconds(fireRate);
                isShootingRight = false;
                break;
        }
    }
    private IEnumerator ShootLeft(Slot equipSlot)
    {
        float fireRate = equipSlot.item.fireRate;
        switch (equipSlot.item.firingMode)
        {
            case Item.FiringMode.auto:
                isShootingLeft = true;
                Combat.FireBullet(equipSlot);
                yield return new WaitForSeconds(1 / fireRate);
                isShootingLeft = false;
                yield return new WaitForSeconds(0.01f);
                StartCoroutine(ShootLeft(equipSlot));
                break;

            case Item.FiringMode.burst:
                isShootingLeft = true;
                Combat.FireBullet(equipSlot);
                yield return new WaitForSeconds(0.1f);
                Combat.FireBullet(equipSlot);
                yield return new WaitForSeconds(0.1f);
                Combat.FireBullet(equipSlot);
                yield return new WaitForSeconds(1 / fireRate);
                isShootingLeft = false;
                yield return new WaitForSeconds(0.01f);
                StartCoroutine(ShootLeft(equipSlot));
                break;

            case Item.FiringMode.semi:
                isShootingLeft = true;
                Combat.FireBullet(equipSlot);
                yield return new WaitForSeconds(0.05f);
                isShootingLeft = false;
                break;

            case Item.FiringMode.single:
                isShootingLeft = true;
                Combat.FireBullet(equipSlot);
                yield return new WaitForSeconds(fireRate);
                isShootingLeft = false;
                break;
        }
    }
    #endregion

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

    #region CAMERA/CHARACTER CONTROLS
    public void CameraControl()
    {
        float x = playerInputs.Player.Mouse.ReadValue<Vector2>().x;
        float y = -playerInputs.Player.Mouse.ReadValue<Vector2>().y;

        followTransform.rotation *= Quaternion.AngleAxis(x * sensitivity/100, Vector3.up);
        followTransform.rotation *= Quaternion.AngleAxis(y * sensitivity/100, Vector3.right);

        var angles = followTransform.eulerAngles;
        angles.z = 0;
        var angle = followTransform.eulerAngles.x;

        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 40)
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

    #endregion

    #region UTILITY
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
    public void RefreshIK()
    {
        rightArmIK.enabled = false;
        leftArmIK.enabled = false;

        rightArmIK.enabled = true;
        leftArmIK.enabled = true;
    }
    #endregion

    private void OnDisable()
    {
        playerInputs.Player.Disable();
    }
}
