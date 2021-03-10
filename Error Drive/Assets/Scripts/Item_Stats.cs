using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Stats : MonoBehaviour
{
    public Weapon weapon;
    public Armor armor;
    
    public int levelRequirement, damage, knockback, health, power, pen, crit, defense;
    public Color itemRarity;
    public Sprite sprite;
    
    // Start is called before the first frame update
    void Start()
    {
        if (weapon)
        {
            levelRequirement = weapon.levelRequirement;
            knockback = weapon.knockback;
            health = weapon.health;
            power = weapon.power;
            pen = weapon.pen;
            crit = weapon.crit;
            defense = weapon.defense;
            damage = weapon.damage;
            itemRarity = weapon.rarityColor;
            sprite = weapon.weaponSprite;
        }
        else if (armor)
        {
            levelRequirement = armor.levelRequirement;
            health = armor.health;
            power = armor.power;
            pen = armor.pen;
            crit = armor.crit;
            defense = armor.defense;
            itemRarity = armor.rarityColor;
            sprite = armor.armorSprite;
        }
        
    }   
    
}
