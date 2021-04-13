using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class Combat
{
    public static GameObject combatText_Prefab = Resources.Load<GameObject>(Path.Combine("Prefabs", "CombatText"));
    public static GameObject sparks_Prefab = Resources.Load<GameObject>(Path.Combine("Particles", "Sparks_Particle"));
    public static Transform worldCanvas = GameObject.Find("World_Canvas").transform;

    public static void DamageTarget(Stats targetStats, Stats myStats)
    {
        int damageDealt = myStats.power * (myStats.armorPen / targetStats.defense);
        targetStats.health -= damageDealt;
        targetStats.gameObject.GetComponent<Rigidbody>().AddForce((targetStats.transform.position - myStats.transform.position).normalized * myStats.knockback, ForceMode.Impulse);
        SpawnCombatText(Color.yellow, damageDealt, 1.5f, targetStats.transform.position + new Vector3(0,3,0));
        GameObject sparks = Object.Instantiate(sparks_Prefab, targetStats.transform.position + new Vector3(0, 3, 0), Quaternion.identity, targetStats.transform);
        Object.Destroy(sparks, 3);
    }

    public static void SpawnCombatText(Color _color, int _damage, float _duration, Vector3 _location)
    {
        GameObject obj = Object.Instantiate(combatText_Prefab);
        CombatText.CombatTextInfo(_color, _damage, _duration, _location); 
    }
}
