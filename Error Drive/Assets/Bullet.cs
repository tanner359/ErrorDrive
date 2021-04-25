using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody rb;
    public Stats sharedStats;
    private void Start()
    {
        Destroy(gameObject, 3f);
    } 
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Hostile"))
        {
            Debug.Log("Hostile Hit");
            Combat.DamageTarget(other.gameObject.GetComponent<Stats>(), sharedStats);
            other.gameObject.GetComponent<Enemy>().DisableAgent();
            other.gameObject.GetComponent<Rigidbody>().AddForce(((transform.position - other.transform.position) * sharedStats.knockback) + Vector3.up * 2, ForceMode.Impulse);          
            Destroy(gameObject);
        }
        Destroy(gameObject);
    }
}
