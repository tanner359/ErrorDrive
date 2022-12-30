using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    public Player player;

    [Header("References")]
    public Player_Inputs playerInputs;
    public Rigidbody rb;
    public Transform followTransform;
    public Stats stats;
    public Inventory inventory;

    [Header("Movement Settings")]    
    Vector3 moveDirection;
    float moveX;
    float moveZ;
    
    [Header("Control Settings")]
    public float sensitivity = 100f;
    public bool isControlling;
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
        ItemPackage items = new ItemPackage(InventorySystem.GetItemsEquipped());
        Equipment equipment = new Equipment(items);

        player = new Player(gameObject, stats, equipment);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void FixedUpdate()
    {
        if (isControlling)
        {          
            moveDirection = moveX * transform.forward + moveZ * transform.right;
            rb.velocity = new Vector3(moveDirection.x * stats.speed, rb.velocity.y, moveDirection.z * stats.speed);
            CameraControl();
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
        }
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
            run = true;
        }
        else if (run)
        {
            stats.speed /= 1.60f;
            run = false;
        }      
        #endregion
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

    #region TIMERS
    float R_Delay = 0;
    float L_Delay = 0;
    public void Timers()
    {
        if(R_Delay > 0)
        {
            R_Delay -= Time.deltaTime;
        }
        if(L_Delay > 0)
        {
            L_Delay -= Time.deltaTime;
        }
    }
    #endregion
    private void OnDisable()
    {
        playerInputs.Player.Disable();
    }
}
