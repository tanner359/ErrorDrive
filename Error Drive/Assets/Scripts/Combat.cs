using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.AI;

public static class Combat
{
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
