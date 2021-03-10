using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor", menuName = "Armor")]
public class Armor : ScriptableObject
{
    public Sprite armorSprite;
    public Material armorMaterial;
    public string armorName;
    public int levelRequirement;
    public int health;
    public int power;
    public int pen;
    public int crit;
    public int defense;
    public string description;
    public string tag;
    public Color rarityColor;  
}
