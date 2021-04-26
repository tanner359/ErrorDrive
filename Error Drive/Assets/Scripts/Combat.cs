using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public static class Combat
{
    public static GameObject combatText_Prefab = Resources.Load<GameObject>(Path.Combine("Prefabs", "CombatText"));
    public static GameObject sparks_Prefab = Resources.Load<GameObject>(Path.Combine("Particles", "Sparks"));
    public static Transform worldCanvas = GameObject.Find("World_Canvas").transform;
    public static Stats player = GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>();

    public static void DamageTarget(Stats targetStats, Stats myStats)
    {
        int damageDealt = myStats.power / (targetStats.defense / myStats.armorPen);
        targetStats.health -= damageDealt;
        SpawnCombatText(Color.red, damageDealt, 1.5f, targetStats.transform.position + new Vector3(0,3,0));
        GameObject sparks = Object.Instantiate(sparks_Prefab, targetStats.transform.position + new Vector3(0, 3, 0), Quaternion.identity, targetStats.transform);
        Object.Destroy(sparks, 3);   
    }
    public static void SpawnCombatText(Color _color, int _damage, float _duration, Vector3 _location)
    {
        CombatText.CombatTextInfo(_color, _damage, _duration);
        Object.Instantiate(combatText_Prefab, _location, player.transform.rotation, worldCanvas);        
    }

    public static void FireBullet(Slot equipSlot)
    {
        Item weapon = equipSlot.item;
        GameObject bulletPrefab = weapon.bullet;
        Transform equipPoint = equipSlot.gameObject.GetComponent<Equip>().bodyParts[0].gameObject.transform;
        GameObject bullet = Object.Instantiate(bulletPrefab, equipPoint.position, Quaternion.identity);
        bullet.transform.LookAt(Reticle.instance.transform);
        bullet.GetComponent<Bullet>().sharedStats = player;
        bullet.GetComponent<Rigidbody>().velocity = (bullet.transform.forward * 100f);
        AudioSource.PlayClipAtPoint(equipSlot.item.shotSound, equipPoint.position);
    }
}


