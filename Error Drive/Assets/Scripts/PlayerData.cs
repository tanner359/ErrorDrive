using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int health;
    public int defense;
    public int armorPen;
    public int critChance;
    public int power;

    public PlayerData(Player_Stats Playerdata)
    {
        health = Playerdata.health;
        defense = Playerdata.defense;
        armorPen = Playerdata.armorPen;
        critChance = Playerdata.critChance;
        power = Playerdata.power;
    }
}
