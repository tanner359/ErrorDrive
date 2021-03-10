using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature_Stats : MonoBehaviour
{
    public Creature creature;
    public int health = 9999;
    public int level = 9999;
    public int damage;
    public int knockback;
    bool isDead = false;
    bool itemDropped = false;
    public Animator animator;
    public ItemSpawnHandler spawnHandler;
    // Start is called before the first frame update
    void Start()
    {
        spawnHandler = GameObject.Find("ItemSpawnHandler").GetComponent<ItemSpawnHandler>();
        health = creature.health;
        level = creature.level;
        damage = creature.damage;
        knockback = creature.knockback;
    }
    void Update()
    {
        if (health <= 0)
        {
            isDead = true;
            animator.SetBool("Death", true);
            gameObject.layer = 14;
            gameObject.GetComponent<CreatureBehavior>().enabled = false;
            Destroy(gameObject, 8f);
            if (isDead && !itemDropped)
            {
                calculateDrops();
            }
        }
    }
    public void calculateDrops()
    {
        int randomNum = Random.Range(0, 100);
        if (randomNum < 5)
        {
            int n = Random.Range(0, creature.armorDrops.Count);
            spawnHandler.spawnItem(creature.armorDrops[n], null, transform);
        }
        if (randomNum < 10 && randomNum > 5)
        {
            int n = Random.Range(0, creature.weaponDrops.Count);
            spawnHandler.spawnItem(null, creature.weaponDrops[n], transform);
        }
        itemDropped = true;
    }
}
