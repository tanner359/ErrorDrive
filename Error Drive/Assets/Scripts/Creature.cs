using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class Creature : ScriptableObject
{
    public string enemyName;
    public int level;
    public Sprite creatureSprite;
    public int damage;
    public string description;
    public Material creatureMaterial;
    public int knockback;
    public int health;
    public float movementSpeed;
    public float runningSpeed;
    public bool isHostile;
    public float aggroDistance;
    public List<Weapon> weaponDrops;
    public List<Armor> armorDrops;
}
