using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public GameObject gameObject;
    public Stats stats;
    public Equipment equipment;

    public Player(GameObject _gameObject, Stats _stats, Equipment _equipment)
    {
        gameObject = _gameObject;
        stats = _stats;
        equipment = _equipment;        
    }
}

public class Equipment
{
    public Dictionary<Item.EquipSlot, Item> items = new Dictionary<Item.EquipSlot, Item>();
    public Equipment(ItemPackage itemPackage)
    {
        items.Add(Item.EquipSlot.Main_Hand, itemPackage.items[0]);
        items.Add(Item.EquipSlot.Off_Hand, itemPackage.items[1]);
        items.Add(Item.EquipSlot.Torso, itemPackage.items[2]);
        items.Add(Item.EquipSlot.Head, itemPackage.items[3]);
        items.Add(Item.EquipSlot.R_Leg, itemPackage.items[4]);
        items.Add(Item.EquipSlot.L_Leg, itemPackage.items[5]);
    }
}
public class ItemPackage 
{
    public Item[] items;
    public ItemPackage(Item MainHand, Item OffHand, Item Torso, Item Head, Item R_Leg, Item L_Leg)
    {
        items = new Item[] { MainHand, OffHand, Torso, Head, R_Leg, L_Leg };
    }

    public ItemPackage(Item[] _items)
    {
        items = _items;
    }
}

