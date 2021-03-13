using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor", menuName = "Armor")]
public class Armor : ScriptableObject
{
    public enum EquipType { Body, Left_Leg, Right_Leg, Head }
    public enum RarityType { Common, Uncommon, Rare, Epic, Legendary }

    [Header("Stats")]
    public int levelRequirement;
    [Space(5)]
    public int health;
    public int power;
    public int pen;
    public int crit;
    public int defense;

    [Header("Info")]
    public string armorName;
    public string description;
    public Mesh mesh;
    public Material material;
    public Sprite sprite;
    public EquipType equipSlot;
    public RarityType rarity;
}
