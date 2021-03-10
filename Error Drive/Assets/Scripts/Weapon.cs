using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{    
    public int damage;
    public int knockback;
    public int levelRequirement;
    public int health;
    public int power;
    public int pen;
    public int crit;
    public int defense;
    public string weaponName;
    public string description;
    public string tag;
    public Material material;
    public Sprite weaponSprite;
    public Mesh Model;
    public Color rarityColor;

}
