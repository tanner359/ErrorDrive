using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Stats : MonoBehaviour
{
    public int health;
    public int defense;
    public int armorPen;
    public int critChance;
    public int power;

    public float saveInterval = 30f;

    private void Awake()
    {
        LoadPlayer();
    }
    private void Update()
    {
        if(saveInterval <= 0)
        {
            SavePlayer();
            saveInterval = 30f;
        }
        else
        {
            saveInterval -= Time.deltaTime;
        }
    }
    public void SavePlayer()
    {
        SaveSystem.savePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.loadPlayer();

        health = data.health;
        defense = data.defense;
        armorPen = data.armorPen;
        critChance = data.critChance;
        power = data.power;
    }    
}

