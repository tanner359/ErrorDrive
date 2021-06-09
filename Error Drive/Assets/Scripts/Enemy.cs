using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Player player;

    public Equipment equipment;

    public Stats stats;
    public enum AIState { Wander, Aggressive }
    public enum AggroType { wander, chase }

    public NavMeshAgent agent;

    public Rigidbody rb;

    public Transform head;

    public GameObject currentTarget;

    public float wanderDistance = 10;

    public float aggroDistance = 10;

    public LayerMask targetMask;

    public Animator animator;

    float R_ClickValue;
    float L_ClickValue;

    [Header("Current State")]
    public AIState currentState;
    public AggroType aggroType;

    #region AIMING AND EQUIPMENT
    [Header("Items Equipped")]
    public Item MainHand;
    public Item OffHand;
    public Item Torso;
    public Item Head;
    public Item R_Leg;
    public Item L_Leg;

    [Header("Aiming")]
    public bool aimRight;
    public bool aimLeft;
    public Transform rightArmCTRL;
    public Transform leftArmCTRL;
    public InverseKinematics rightArmIK;
    public InverseKinematics leftArmIK;
    #endregion

    public void Awake()
    {
        ItemPackage items = new ItemPackage(MainHand, OffHand, Torso, Head, R_Leg, L_Leg);
        Equipment equipment = new Equipment(items);
        player = new Player(gameObject, stats, equipment);
    }

    public void Start()
    {
        currentState = AIState.Wander;       
        RefreshIK();
    }

    
    private void LateUpdate()
    {
        if (agent.enabled && disabledTime <= 0)
        {
            StateController();
        }
    }

    private void FixedUpdate()
    {
        if(disabledTime > 0 || pathTime > 0) { Timers(); }  
    }

    public void Update()
    {     
        AnimationControls();
    }

    public void StateController()
    {       
        switch (currentState)
        {
            case AIState.Wander:
                currentTarget = FindTarget();
                if (currentTarget != null){currentState = AIState.Aggressive;break;}
                if(pathTime <= 0){ Wander(); agent.autoBraking = true; break; }
                break;

            case AIState.Aggressive:
                if(currentTarget != null) { Aggro(currentTarget); break; }
                else if (!losingAggro) { StartCoroutine(DeAggro()); }                               
                break;
        }
    }
    public void Wander()
    {
        agent.SetDestination(transform.position);
        pathTime = Random.Range(2, 10);      
        Vector3 point = GetRandomDestination();
        agent.SetDestination(point);
    }
    public void Aggro(GameObject target)
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);
        if (distance > aggroDistance) { aggroType = AggroType.chase; }
        else { aggroType = AggroType.wander;}        
        SetAim();
        switch (aggroType)
        {
            case AggroType.chase:
                agent.autoBraking = false;
                agent.SetDestination(target.transform.position);
                break;

            case AggroType.wander:
                transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
                if(pathTime <= 0) { Wander(); agent.autoBraking = true; }              
                break;
        }

        if (currentTarget != null)
        {
            if (MainHand && !isShootingRight && R_ClickValue == 0)
            {
                StartCoroutine(RandomRightClick());
                StartCoroutine(ShootRight(player, MainHand));
            }
            if (OffHand && !isShootingLeft && L_ClickValue == 0)
            {
                StartCoroutine(RandomLeftClick());
                StartCoroutine(ShootLeft(player, OffHand));
            }
        }
    }

    bool losingAggro;
    public IEnumerator DeAggro()
    {
        losingAggro = true;
        yield return new WaitForSeconds(5f);
        if (FindTarget() == null) { currentState = AIState.Wander; currentTarget = null; }
        losingAggro = false;
    }
    public IEnumerator RandomLeftClick()
    {
        L_ClickValue = 1;
        yield return new WaitForSeconds(Random.Range(0.5f, 1f));
        L_ClickValue = 0;
    }
    public IEnumerator RandomRightClick()
    {
        R_ClickValue = 1;
        yield return new WaitForSeconds(Random.Range(0.5f, 1f));
        R_ClickValue = 0;
    }
    public void AnimationControls()
    {
        if (agent.enabled)
        {
            if (agent.remainingDistance == 0)
            {
                animator.SetBool("Walk", false);
                animator.SetBool("Run", false);
                animator.SetBool("StrafeRight", false);
                animator.SetBool("StrafeLeft", false);
                animator.SetBool("WalkBackwards", false);
                return;
            }
        }       

        switch (currentState)
        {
            case AIState.Wander:               
                animator.SetBool("Walk", true);
                break;

            case AIState.Aggressive:
                switch (aggroType)
                {
                    case AggroType.chase:
                        animator.SetBool("Run", true);
                        break;
                    case AggroType.wander:
                        float angle = Vector3.SignedAngle(transform.forward, agent.pathEndPosition - transform.position, Vector3.up);
                        if(angle > 30f && angle < 150f)
                        {
                            animator.SetBool("StrafeRight", true);
                            animator.SetBool("StrafeLeft", false);
                            animator.SetBool("WalkBackwards", false);
                            break;
                        }
                        else if( angle < -30f && angle > -150f)
                        {
                            animator.SetBool("StrafeLeft", true);
                            animator.SetBool("WalkBackwards", false);
                            animator.SetBool("StrafeRight", false);

                            break;
                        }
                        else if(angle < -150f || angle > 150f)
                        {
                            animator.SetBool("WalkBackwards", true);
                            animator.SetBool("StrafeLeft", false);
                            animator.SetBool("StrafeRight", false);


                            break;
                        }
                        animator.SetBool("Walk", true);
                        animator.SetBool("WalkBackwards", false);
                        animator.SetBool("StrafeLeft", false);
                        animator.SetBool("StrafeRight", false);
                        break;
                }
                break;
        }      
    }

    #region TIMERS
    float pathTime;
    public float disabledTime;
    private void Timers()
    {
        #region DISABLED TIMER
        if (disabledTime > 0)
        {
            disabledTime -= Time.deltaTime;
        }
        else if (disabledTime <= 0 && rb.velocity == Vector3.zero && !agent.enabled)
        {
            EnableAgent();
        }
        #endregion

        #region PATH TIMER
        if (pathTime >= 0)
        {
            pathTime -= Time.deltaTime;
        }
        #endregion
    }
    #endregion

    #region UTILITY
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(agent.pathEndPosition, 0.5f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggroDistance);
    }

    public void SetAim()
    {
        if (MainHand && OffHand)
        {
            leftArmCTRL.position = currentTarget.transform.position + Vector3.up * 2;
            rightArmCTRL.position = currentTarget.transform.position + Vector3.up * 2;
        }
        else if (MainHand)
        {
            rightArmCTRL.position = currentTarget.transform.position + Vector3.up * 2;
        }
        else if (OffHand)
        {
            leftArmCTRL.position = currentTarget.transform.position + Vector3.up * 2;
        }
    }
    public void DisableAgent()
    {
        agent.enabled = false;
        rb.isKinematic = false;
        disabledTime = 1f;
        animator.SetTrigger("Damaged");
    }
    public void EnableAgent()
    {
        agent.enabled = true;
        rb.isKinematic = true;
        currentState = AIState.Wander;
    }
    public Vector3 GetRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderDistance;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, wanderDistance, 1);
        return hit.position;      
    }
    public void RefreshIK()
    {
        rightArmIK.enabled = false;
        leftArmIK.enabled = false;

        rightArmIK.enabled = true;
        leftArmIK.enabled = true;
    }
    public GameObject FindTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, aggroDistance, targetMask);
        GameObject player = null;
        for (int i = 0; i < colliders.Length-1; i++)
        {
            player = colliders[i].gameObject;
            Physics.Raycast(head.transform.position, player.transform.position - transform.position, out RaycastHit hit, aggroDistance);
            if(hit.transform.gameObject.layer != 6){ return null; }                        
        }
        return player;
    }

    bool isShootingRight;
    bool isShootingLeft;
    private IEnumerator ShootRight(Player player, Item weapon)
    {
        float fireRate = weapon.fireRate;
        switch (weapon.firingMode)
        {
            case Item.FiringMode.auto:
                isShootingRight = true;
                Combat.FireBullet(player, weapon);
                yield return new WaitForSeconds(1 / fireRate);
                yield return new WaitForSeconds(0.01f);
                if (R_ClickValue == 1) { StartCoroutine(ShootRight(player, weapon)); }
                else { isShootingRight = false; }
                break;

            case Item.FiringMode.burst:
                isShootingRight = true;
                Combat.FireBullet(player, weapon);
                yield return new WaitForSeconds(0.1f);
                Combat.FireBullet(player, weapon);
                yield return new WaitForSeconds(0.1f);
                Combat.FireBullet(player, weapon);
                yield return new WaitForSeconds(1 / fireRate);
                yield return new WaitForSeconds(0.01f);
                if (R_ClickValue == 1) { StartCoroutine(ShootRight(player, weapon)); }
                else { isShootingRight = false; }
                break;

            case Item.FiringMode.semi:
                isShootingRight = true;
                Combat.FireBullet(player, weapon);
                yield return new WaitForSeconds(0.05f);
                isShootingRight = false;
                break;

            case Item.FiringMode.single:
                isShootingRight = true;
                Combat.FireBullet(player, weapon);
                yield return new WaitForSeconds(fireRate);
                isShootingRight = false;
                break;
        }
    }
    private IEnumerator ShootLeft(Player player, Item weapon)
    {
        float fireRate = weapon.fireRate;
        switch (weapon.firingMode)
        {
            case Item.FiringMode.auto:
                isShootingLeft = true;
                Combat.FireBullet(player, weapon);
                yield return new WaitForSeconds(1 / fireRate);
                yield return new WaitForSeconds(0.01f);
                if (L_ClickValue == 1) { StartCoroutine(ShootLeft(player, weapon)); }
                else { isShootingLeft = false; }
                break;

            case Item.FiringMode.burst:
                isShootingLeft = true;
                Combat.FireBullet(player, weapon);
                yield return new WaitForSeconds(0.1f);
                Combat.FireBullet(player, weapon);
                yield return new WaitForSeconds(0.1f);
                Combat.FireBullet(player, weapon);
                yield return new WaitForSeconds(1 / fireRate);
                yield return new WaitForSeconds(0.01f);
                if (L_ClickValue == 1) { StartCoroutine(ShootLeft(player, weapon)); }
                else { isShootingLeft = false; }
                break;

            case Item.FiringMode.semi:
                isShootingLeft = true;
                Combat.FireBullet(player, weapon);
                yield return new WaitForSeconds(0.05f);
                isShootingLeft = false;
                break;

            case Item.FiringMode.single:
                isShootingLeft = true;
                Combat.FireBullet(player, weapon);
                yield return new WaitForSeconds(fireRate);
                isShootingLeft = false;
                break;
        }
    }

    #endregion
}