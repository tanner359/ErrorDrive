using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody rb;
    public Stats sharedStats;
    public Item weapon;
    private LayerMask enemyMask;

    [Header("Optional")]
    public GameObject explosion;
    public AudioClip soundClip;
    private void Start()
    {
        enemyMask = LayerMask.GetMask("Hostiles");
        Destroy(gameObject, 3f);
    } 
    private void OnCollisionEnter(Collision other)
    {
        if(explosion != null)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(soundClip, transform.position);
            Collider[] hostilesHit = Physics.OverlapSphere(transform.position, 6f, enemyMask);
            if(hostilesHit.Length > 0)
            {
                for(int i = 0; i < hostilesHit.Length; i++)
                {
                    Combat.DamageTarget(weapon, hostilesHit[i].gameObject.GetComponent<Stats>(), sharedStats);
                    hostilesHit[i].GetComponent<Rigidbody>().AddForce(((transform.position - hostilesHit[i].gameObject.transform.position) * weapon.knockback) + Vector3.up * 2, ForceMode.Impulse);
                }
            }
        }
        if (other.gameObject.CompareTag("Hostile"))
        {
            Combat.DamageTarget(weapon, other.gameObject.GetComponent<Stats>(), sharedStats);
            other.gameObject.GetComponent<Enemy>().DisableAgent();
            other.gameObject.GetComponent<Rigidbody>().AddForce(((transform.position - other.transform.position) * weapon.knockback) + Vector3.up * 2, ForceMode.Impulse);          
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            Combat.DamageTarget(weapon, other.gameObject.GetComponent<Stats>(), sharedStats);
            other.gameObject.GetComponent<Rigidbody>().AddForce(((transform.position - other.transform.position) * weapon.knockback) + Vector3.up * 2, ForceMode.Impulse);
            Destroy(gameObject);
        }
        Destroy(gameObject);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 6f);
    }
}


