using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [Header("Offense")]
    public int power;
    public int armorPen;
    public int critChance;
    

    [Header("Defense")]
    public int health;
    public int defense;

    [Header("Utility")]
    public float speed = 1f;
    public int knockback;
}

