using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Stats : MonoBehaviour
{
    public Weapon weapon;
    public Armor armor;
    
    public int levelRequirement, damage, knockback, health, power, pen, crit, defense;
    public object rarity;
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
            rarity = weapon.rarity;
            sprite = weapon.sprite;
        }
        else if (armor)
        {
            levelRequirement = armor.levelRequirement;
            health = armor.health;
            power = armor.power;
            pen = armor.pen;
            crit = armor.crit;
            defense = armor.defense;
            rarity = armor.rarity;
            sprite = armor.sprite;
        }
        
    }   
    
}
