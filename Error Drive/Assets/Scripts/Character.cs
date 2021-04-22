using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class Character : ScriptableObject
{
    public string creatureName;
    public int level;
    public string description;
    public GameObject prefab;

    [Header("Offense")]
    public int power;
    public int armorPenetration;
    public int criticalStrike;

    [Header("Defense")]
    public int health;
    public int defense;

    [Header("Utility")]
    public float movementSpeed;
    public float runningSpeed;

    [Header("Drops")]
    public List<Object> items;

    [Header("Extra Options")]
    public bool isHostile;
    public float aggroDistance; 
}
