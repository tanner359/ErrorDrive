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

    public Dictionary<Item.EquipType, Item> items;
    public Dictionary<Item.EquipType, Transform> bodyParts;
    public Equipment(ItemPackage itemPackage, BodyPartsPackage bodyPartsPackage)
    {
        items.Add(Item.EquipType.Main_Hand, itemPackage.items[0]);
        items.Add(Item.EquipType.Off_Hand, itemPackage.items[1]);
        items.Add(Item.EquipType.Torso, itemPackage.items[2]);
        items.Add(Item.EquipType.Head, itemPackage.items[3]);
        items.Add(Item.EquipType.R_Leg, itemPackage.items[4]);
        items.Add(Item.EquipType.L_Leg, itemPackage.items[4]);

        bodyParts.Add(Item.EquipType.Main_Hand, bodyPartsPackage.bodyParts[0]);
        bodyParts.Add(Item.EquipType.Off_Hand, bodyPartsPackage.bodyParts[1]);
        bodyParts.Add(Item.EquipType.Torso, bodyPartsPackage.bodyParts[2]);
        bodyParts.Add(Item.EquipType.Head, bodyPartsPackage.bodyParts[3]);
        bodyParts.Add(Item.EquipType.R_Leg, bodyPartsPackage.bodyParts[4]);
        bodyParts.Add(Item.EquipType.L_Leg, bodyPartsPackage.bodyParts[5]);
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

public class BodyPartsPackage
{
    public Transform[] bodyParts;
    public BodyPartsPackage(Transform R_ForeArm, Transform L_ForeArm, Transform Torso, Transform Head, 
        Transform R_UpperLeg, Transform R_LowerLeg, Transform L_UpperLeg, Transform L_LowerLeg)
    {
        bodyParts = new Transform[] { R_ForeArm, L_ForeArm, Torso, Head, R_UpperLeg, R_LowerLeg, L_UpperLeg, L_LowerLeg };
    }
}

