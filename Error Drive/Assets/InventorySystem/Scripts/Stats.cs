using UnityEngine;

public class Stats : MonoBehaviour
{
    public Item source;

    [Header("Offense")]
    public int power;
    public int armorPen;
    public int critChance;  

    [Header("Defense")]
    public int health;
    public int maxHealth;
    public int defense;

    [Header("Utility")]
    public float speed = 1f;
    public int knockback;

    public void SetStats(int _power, int _pen, int _crit, int _health, int _maxHealth, int _defense, float _speed, int _knockback)
    {
        power = _power;
        armorPen = _pen;
        critChance = _crit;

        health = _health;
        maxHealth = _maxHealth;
        defense = _defense;

        speed = _speed;
        knockback = _knockback;
    }

    public void SetStats(Item item)
    {
        power = item.power;
        armorPen = item.pen;
        critChance = item.crit;
        maxHealth = item.health;
        defense = item.defense;
        speed = item.speed;
        source = item;
    }
    
    public void AddStats(Item item)
    {
        power += item.power;
        armorPen += item.pen;
        critChance += item.crit;
        maxHealth += item.health;
        defense += item.defense;
        speed += item.speed;
    }

    public void RemoveStats(Item item)
    {
        power -= item.power;
        armorPen -= item.pen;
        critChance -= item.crit;
        maxHealth -= item.health;
        defense -= item.defense;
        speed -= item.speed;
    }
}

