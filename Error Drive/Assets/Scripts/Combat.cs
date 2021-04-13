using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class Combat
{
    public static GameObject combatText_Prefab = Resources.Load<GameObject>(Path.Combine("Prefabs", "CombatText"));
    public static Transform worldCanvas = GameObject.Find("World_Canvas").transform;
    public static void DamageTarget(Stats targetStats, Stats myStats)
    {
        int damageDealt = myStats.power / (targetStats.defense) - myStats.armorPen;
        targetStats.health -= damageDealt;
        SpawnCombatText(new CombatText(Color.yellow, damageDealt, 1.5f, targetStats.GetComponent<Transform>().position));
    }

    public static void SpawnCombatText(CombatText combatText)
    {
        Object.Instantiate(combatText_Prefab);
    }
}
