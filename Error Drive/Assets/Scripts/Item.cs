using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public enum EquipType { Torso, Legs, Head , Main_Hand, Off_Hand }
    public enum RarityType { Common, Uncommon, Rare, Epic, Legendary }
    public enum ItemClass { Ranged, Melee, Armor }
    public enum FiringMode { auto, semi, burst, single }

    [Header("Stats")]
    public int levelRequirement;
    [Space(5)]
    public int health;
    public int power;
    public int pen;
    public int crit;
    public int defense;
    public float speed;
    public int knockback;

    [Header("Info")]
    public string itemName;
    public string description;
    public Mesh mesh;
    public Material material;
    public Sprite sprite;
    public EquipType equipSlot;
    public RarityType rarity;
    public ItemClass itemClass;
    [Header("RANGED ONLY!")]
    public GameObject bullet;
    public FiringMode firingMode;
    [Range(1, 10)]
    public float fireRate;
}
