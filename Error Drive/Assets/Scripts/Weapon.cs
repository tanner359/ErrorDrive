using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public enum EquipType { Main_Hand, Off_Hand }
    public enum RarityType { Common, Uncommon, Rare, Epic, Legendary }


    [Header("Stats")]
    public int levelRequirement;
    [Space(5)]
    public int damage;
    public int knockback;   
    public int health;
    public int power;
    public int pen;
    public int crit;
    public int defense;

    [Header("Info")]
    public string weaponName;
    public string description;
    public Mesh mesh;
    public Material material;
    public Sprite sprite;
    public EquipType equipSlot;
    public RarityType rarity;

}
