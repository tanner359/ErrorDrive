using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public static class Combat
{
    public static GameObject combatText_Prefab = Resources.Load<GameObject>(Path.Combine("Prefabs", "CombatText"));
    public static GameObject sparks_Prefab = Resources.Load<GameObject>(Path.Combine("Particles", "Sparks"));
    public static Transform worldCanvas = GameObject.Find("ScreenCanvas").transform;
    public static Stats player = GameObject.Find("Player2").GetComponent<Stats>();

    public static void DamageTarget(Item item, Stats targetStats, Stats myStats)
    {
        int damageDealt = item.baseDamage + myStats.power / (targetStats.defense - myStats.armorPen) + 1;
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

    public static void FireBullet(Player player, Item weapon)
    {
        GameObject bulletPrefab = weapon.bullet;
        player.gameObject.GetComponent<EquipmentLink>().bodyLinks.TryGetValue(weapon.equipSlot, out List<GameObject> links);
        GameObject bullet = Object.Instantiate(bulletPrefab, links[0].transform.position, Quaternion.identity);
        Physics.IgnoreCollision(bullet.GetComponent<SphereCollider>(), player.gameObject.GetComponent<CapsuleCollider>(), true);
        if (player.gameObject.CompareTag("Player")) { bullet.transform.LookAt(Reticle.instance.transform); }
        else { bullet.transform.LookAt(links[0].transform.position + links[0].transform.forward); }
        bullet.GetComponent<Bullet>().sharedStats = player.stats;
        bullet.GetComponent<Bullet>().weapon = weapon;
        bullet.GetComponent<Rigidbody>().velocity = (bullet.transform.forward * 100f);
        AudioSource.PlayClipAtPoint(weapon.shotSound, links[0].transform.position);
    }
}


