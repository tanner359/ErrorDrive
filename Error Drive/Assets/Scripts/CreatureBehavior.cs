using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureBehavior : MonoBehaviour
{
    public Creature creature;
    public Creature_Stats stats;
    GameObject player;
    public Animator animator;
    public CanvasDisplay canvasDisplay;
    // Start is called before the first frame update

    public float moveDuration = 2f;
    public float waitSeconds = 3f;
    float moveDirection;
    float wanderSpeed;

    void Start()
    {
        canvasDisplay = GameObject.Find("Canvas_Display").GetComponent<CanvasDisplay>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckAggro())
        {
            
            if (GetDistanceFromPlayer() <= 2)
            {
                animator.SetBool("isAttack", true);
            }
            else
            {
                float run = creature.runningSpeed * Time.deltaTime;
                moveDirection = GetDirection(player.transform, gameObject.transform);
                SetDirection();
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, run);
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", true);
                animator.SetBool("isAttack", false);
            }
        }      
        else
        {
            animator.SetBool("isRunning", false);            
            WanderCycle();
        }
        
       
    }
    public void Attack()
    {
        if(GetDistanceFromPlayer() < 2)
        {
            player.GetComponent<Player_Stats>().health -= stats.damage;
            canvasDisplay.displayDamageText(Color.magenta, -stats.damage, 2f, player.transform);
            player.GetComponent<Rigidbody2D>().AddForce(new Vector2(moveDirection * stats.knockback, 0), ForceMode2D.Impulse);
        }
    }
    public void SetDirection(){       
        if(moveDirection < 0){
            gameObject.transform.localScale = new Vector3(Mathf.Abs(gameObject.transform.localScale.x), gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        }
        else if(moveDirection > 0){
            gameObject.transform.localScale = new Vector3(-Mathf.Abs(gameObject.transform.localScale.x), gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        }
    }
    public int GetDirection(Transform tran1, Transform tran2){
        if (tran1.position.x - tran2.position.x > 0){
            return 1;
        }
        else{
            return -1;
        }
    }

    public bool CheckAggro()
    {
        if (Mathf.Abs(Mathf.Abs(player.transform.position.x) - Mathf.Abs(gameObject.transform.position.x)) < creature.aggroDistance && isHostile())
        {           
            return true;
        }
        else
        {
            return false;
        }
    }
    public float GetDistanceFromPlayer()
    {
        float distance = Vector2.Distance(player.transform.position, gameObject.transform.position);
        return distance;
    }

    public bool isHostile()
    {
        if (creature.isHostile)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void WanderCycle()
    {     
        if (waitSeconds > 0)
        {
            animator.SetBool("isWalking", false);
            waitSeconds -= Time.deltaTime;
        }
        if(waitSeconds < 0)
        {
            
            SetDirection();

            if (moveDuration > 0 && moveDirection != 0)
            {
                float wanderSpeed = creature.movementSpeed * Time.deltaTime;
                animator.SetBool("isWalking", true);
                moveDuration -= Time.deltaTime;
                gameObject.transform.position = transform.position + (moveDirection * transform.right) * wanderSpeed;
            }
            else
            {
                moveDirection = GetRandomNum(-1, 2);              
                waitSeconds = Random.Range(2f, 10f);
                moveDuration = Random.Range(2, 5);
            }
        }
    }
    public int GetRandomNum(int min, int max)
    {
        int randomNum = Random.Range(min, max);
        if (randomNum == 0)
        {
            randomNum = Random.Range(min, max);
            if (randomNum != 0)
            {
                return randomNum;
            }
        }
        else
        {
            return randomNum;
        }
        return min;
    }


}
