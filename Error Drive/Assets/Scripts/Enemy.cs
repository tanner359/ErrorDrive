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

    private LayerMask playerMask;

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

    [Header("Body Parts")]
    public Transform MainHandMesh;
    public Transform OffHandMesh;

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
        ItemPackage items = new ItemPackage(MainHand, OffHand, null, null, null, null);
        Equipment equipment = new Equipment(items);
        player = new Player(gameObject, stats, equipment);
    }

    public void Start()
    {
        currentState = AIState.Wander;
        playerMask = LayerMask.GetMask("Player");
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
        Timers();  
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
                transform.LookAt(new Vector3(target.transform.position.x, 0, target.transform.position.z));
                if(pathTime <= 0) { Wander(); agent.autoBraking = true; }              
                break;
        }

        if (FindTarget() != null)
        {
            if (MainHand && !isShootingRight && R_ClickValue == 0)
            {
                StartCoroutine(RandomRightClick());
                StartCoroutine(ShootRight(MainHand, MainHandMesh));
            }
            if (OffHand && !isShootingLeft && L_ClickValue == 0)
            {
                StartCoroutine(RandomLeftClick());
                StartCoroutine(ShootLeft(OffHand, OffHandMesh));
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
                        animator.SetBool("Walk", true);
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
        Collider[] colliders = Physics.OverlapSphere(transform.position, aggroDistance, playerMask);
        GameObject player = null;
        for (int i = 0; i < colliders.Length; i++)
        {
            player = colliders[i].gameObject;
            Physics.Raycast(head.transform.position, player.transform.position - transform.position, out RaycastHit hit, aggroDistance);
            Debug.DrawRay(head.transform.position, player.transform.position - transform.position, Color.green);
            if(!hit.collider.gameObject.CompareTag("Player")){return null; }                        
        }
        return player;
    }

    bool isShootingRight;
    bool isShootingLeft;
    private IEnumerator ShootRight(Item weapon, Transform equipPoint)
    {
        float fireRate = weapon.fireRate;
        switch (weapon.firingMode)
        {
            case Item.FiringMode.auto:
                isShootingRight = true;
                Combat.FireBullet(player, weapon);
                yield return new WaitForSeconds(1 / fireRate);
                yield return new WaitForSeconds(0.01f);
                if (R_ClickValue == 1) { StartCoroutine(ShootRight(weapon, equipPoint)); }
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
                if (R_ClickValue == 1) { StartCoroutine(ShootRight(weapon, equipPoint)); }
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
    private IEnumerator ShootLeft(Item weapon, Transform equipPoint)
    {
        float fireRate = weapon.fireRate;
        switch (weapon.firingMode)
        {
            case Item.FiringMode.auto:
                isShootingLeft = true;
                Combat.FireBullet(player, weapon);
                yield return new WaitForSeconds(1 / fireRate);
                yield return new WaitForSeconds(0.01f);
                if (L_ClickValue == 1) { StartCoroutine(ShootLeft(weapon, equipPoint)); }
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
                if (L_ClickValue == 1) { StartCoroutine(ShootLeft(weapon, equipPoint)); }
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
