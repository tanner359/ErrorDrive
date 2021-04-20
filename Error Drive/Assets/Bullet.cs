using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Stats sharedStats;
    void Start()
    {
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Hostile"))
        {
            Combat.DamageTarget(collision.gameObject.GetComponent<Stats>(), sharedStats);
            Destroy(gameObject);
        }
        Destroy(gameObject);
    }
}
