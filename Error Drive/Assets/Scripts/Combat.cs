using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public static class Combat
{
    public static Ranged ranged { get; set; } = new Ranged();
    public static GameObject combatText_Prefab = Resources.Load<GameObject>(Path.Combine("Prefabs", "CombatText"));
    public static GameObject sparks_Prefab = Resources.Load<GameObject>(Path.Combine("Particles", "Sparks_Particle"));
    public static Transform worldCanvas = GameObject.Find("World_Canvas").transform;
    public static Stats player = GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>();

    public static void DamageTarget(Stats targetStats, Stats myStats)
    {
        Rigidbody rb = targetStats.GetComponent<Rigidbody>();
        int damageDealt = myStats.power / (targetStats.defense / myStats.armorPen);
        targetStats.health -= damageDealt;

        if (targetStats.TryGetComponent(out NavMeshAgent agent))
        {
            targetStats.GetComponent<Enemy>().AgentActive(false);
            rb.AddForce(((targetStats.transform.position - myStats.transform.position).normalized * myStats.knockback) + Vector3.up * 5, ForceMode.Impulse);          
        }
        else
        {
            rb.AddForce(((targetStats.transform.position - myStats.transform.position).normalized * myStats.knockback) + Vector3.up * 5, ForceMode.Impulse);
        }
        SpawnCombatText(Color.red, damageDealt, 1.5f, targetStats.transform.position + new Vector3(0,3,0));
        GameObject sparks = Object.Instantiate(sparks_Prefab, targetStats.transform.position + new Vector3(0, 3, 0), Quaternion.identity, targetStats.transform);
        Object.Destroy(sparks, 3);   
    }
    public static void SpawnCombatText(Color _color, int _damage, float _duration, Vector3 _location)
    {
        CombatText.CombatTextInfo(_color, _damage, _duration);
        Object.Instantiate(combatText_Prefab, _location, player.transform.rotation, worldCanvas);        
    }    
}

public class Ranged : MonoBehaviour
{
    public void Shoot(Slot equipSlot)
    {
        switch (equipSlot.item.firingMode)
        {
            case Item.FiringMode.auto:
                StartCoroutine(AutoShot(equipSlot, equipSlot.item.fireRate));
                break;

            case Item.FiringMode.semi:
                StartCoroutine(SemiShot(equipSlot, equipSlot.item.fireRate));
                break;

            case Item.FiringMode.burst:
                StartCoroutine(BurstShot(equipSlot, equipSlot.item.fireRate));
                break;

            case Item.FiringMode.single:
                StartCoroutine(SingleShot(equipSlot, equipSlot.item.fireRate));
                break;
        }
    }
    
    public void StopShooting()
    {
        StopAllCoroutines();
    }

    private void FireBullet(Slot equipSlot)
    {
        Item weapon = equipSlot.item;
        GameObject bulletPrefab = weapon.bullet;
        Transform equipPoint = equipSlot.gameObject.GetComponent<Equip>().bodyParts[0].gameObject.transform;
        GameObject bullet = Instantiate(bulletPrefab, equipPoint.position + equipPoint.transform.forward, equipPoint.transform.rotation);
        bullet.GetComponent<Bullet>().sharedStats = Combat.player;
        bullet.GetComponent<Rigidbody>().velocity = (bullet.transform.forward * 50f);
    }
    private IEnumerator BurstShot(Slot equipSlot, float fireRate)
    {
        FireBullet(equipSlot);
        yield return new WaitForSeconds(1 / fireRate);
    }

    private IEnumerator AutoShot(Slot equipSlot, float fireRate)
    {
        FireBullet(equipSlot);
        yield return new WaitForSeconds(1 / fireRate);
    }

    private IEnumerator SemiShot(Slot equipSlot, float fireRate)
    {
        FireBullet(equipSlot);
        yield return new WaitForSeconds(1 / fireRate);
    }

    private IEnumerator SingleShot(Slot equipSlot, float fireRate)
    {
        FireBullet(equipSlot);
        yield return new WaitForSeconds(1 / fireRate);
    }
}

