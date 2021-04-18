using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;

    public float wanderRange = 10;

    public float aggroRange = 10;

    public Vector3 origin;

    private LayerMask terrainMask;

    public Animator animator;

    public bool wander;

    float pathTimer;

    public void Start()
    {
        terrainMask = LayerMask.GetMask("Terrain");
        origin = transform.position;      
        StartCoroutine(Wander());        
    }

    void Update()
    {
        Search();             
    }

    private void FixedUpdate()
    {
        if (pathTimer >= 0)
        {
            pathTimer -= Time.deltaTime;
        }
    }

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
}
