using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;

    public Rigidbody rb;

    public float wanderRange = 10;

    public float aggroRange = 10;

    public Vector3 origin;

    private LayerMask terrainMask;

    public Animator animator;

    public bool wander;

    public void Start()
    {
        terrainMask = LayerMask.GetMask("Terrain");
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position, out hit, wanderRange, 1);
        origin = hit.position;     
        StartCoroutine(Wander());        
    }

    void Update()
    {
        if (agent.enabled && disabledTime <= 0)
        {
            Search();
        }
    }

    private void FixedUpdate()
    {
        Timers();  
    }

    #region TIMER
    float pathTimer;
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
            Debug.Log("Re-Enable Agent");
            EnableAgent();
        }
        #endregion

        #region PATH TIMER
        if (pathTimer >= 0)
        {
            pathTimer -= Time.deltaTime;
        }
        #endregion
    }
    #endregion

    #region COROUTINES
    public IEnumerator Wander()
    {
        animator.SetBool("Walk", true);
        wander = true;
        agent.autoBraking = true;
        Vector3 point = GetWanderPoint();
        agent.SetDestination(point);
        pathTimer = 0.8f * wanderRange;
        yield return new WaitUntil(() => IsPathCompleted());
        agent.SetDestination(transform.position);
        animator.SetBool("Walk", false);
        yield return new WaitForSeconds(Random.Range(3, 5));
        if (agent.enabled)
        {
            StartCoroutine(Wander());
        }
    }
    #endregion

    #region UTILITY

    public void DisableAgent()
    {
        StopAllCoroutines();
        agent.enabled = false;
        rb.isKinematic = false;
        disabledTime = 1f;
        animator.SetTrigger("Damaged");
    }
    public void EnableAgent()
    {
        agent.enabled = true;
        rb.isKinematic = true;
        StartCoroutine(Wander());       
    }
    public bool IsPathCompleted()
    {
        if(pathTimer <= 0 || agent.remainingDistance < 0.5f)
        {
            return true;
        }
        return false;
    }
    public Vector3 GetWanderPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRange;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, wanderRange, 1);
        Vector3 finalPosition = hit.position;
        return finalPosition;
    }
    public void Search()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, aggroRange);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("Player"))
            {
                animator.SetBool("Run", true);
                wander = false;
                StopAllCoroutines();
                Vector3 target = colliders[i].transform.position;
                agent.autoBraking = false;
                agent.SetDestination(target);
                return;
            }                     
        }

        if (!wander)
        {
            animator.SetBool("Run", false);
            StartCoroutine(Wander());
        }
    }

    #endregion
}
