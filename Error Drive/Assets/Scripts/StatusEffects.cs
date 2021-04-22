using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StatusEffects
{ 
    public static IEnumerator SlowTarget(GameObject target, float amount, float time)
    {
        Stats stats = target.GetComponent<Stats>();
        stats.speed *= (1 - amount);
        yield return new WaitForSeconds(time);
        stats.speed /= (1 - amount);
    }

    public static IEnumerator HastenTarget(GameObject target, float amount, float time)
    {
        Stats stats = target.GetComponent<Stats>();
        stats.speed *= (1 + amount);
        yield return new WaitForSeconds(time);
        stats.speed /= (1 + amount);
    }

}
