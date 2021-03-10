using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSpawnHandler : MonoBehaviour
{
    public Creature creature;
    public GameObject enemy_00;
    public float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 3;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            GameObject.Instantiate(enemy_00, gameObject.transform.position, Quaternion.identity);
            timer = 3f;
        }
    }
}
