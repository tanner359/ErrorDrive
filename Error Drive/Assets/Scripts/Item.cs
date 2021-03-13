using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class Item
{
    public static Object[] AllItems = Resources.LoadAll("Scriptable Objects");
    public static Weapon[] weapons = Resources.LoadAll<Weapon>(Path.Combine("Scriptable Objects", "Weapons"));
    public static Armor[] armor = Resources.LoadAll<Armor>(Path.Combine("Scriptable Objects", "Armor"));
    public static GameObject rarityParticle = Resources.Load<GameObject>(Path.Combine("Particles", "Rarity_Particle"));
    
    public static void Spawn(Armor armor, Vector3 position)
    {
        GameObject item = new GameObject(armor.armorName);
        item.tag = armor.equipSlot.ToString();
        item.layer = 9;
        item.GetComponent<Transform>().position = position;
        item.GetComponent<Transform>().localScale = item.GetComponent<Transform>().localScale / 2f;
        item.AddComponent<Rigidbody>();
        item.AddComponent<MeshFilter>().mesh = armor.mesh;
        item.AddComponent<MeshCollider>().convex = true;
        //item.GetComponent<MeshCollider>().sharedMesh = armor.mesh;
        item.AddComponent<MeshRenderer>().material = armor.material;
        item.AddComponent<Item_Stats>().armor = armor;
        GameObject particle = Object.Instantiate(rarityParticle, item.transform.position, Quaternion.Euler(0, -90, 0), item.transform);
        ParticleSystem.TrailModule settings = particle.GetComponent<ParticleSystem>().trails;
        settings.colorOverTrail = GetRarityColor(armor.rarity);
    }
    public static void Spawn(Weapon weapon, Vector3 position)
    {
        GameObject item = new GameObject(weapon.weaponName);
        item.tag = weapon.equipSlot.ToString();
        item.layer = 9;
        item.GetComponent<Transform>().position = position;
        item.GetComponent<Transform>().localScale = item.GetComponent<Transform>().localScale / 2f;
        item.AddComponent<Rigidbody>();
        item.AddComponent<MeshFilter>().mesh = weapon.mesh;
        item.AddComponent<MeshCollider>().convex = true;
        item.AddComponent<MeshRenderer>().material = weapon.material;
        item.AddComponent<Item_Stats>().weapon = weapon;
        GameObject particle = Object.Instantiate(rarityParticle, item.transform.position, Quaternion.Euler(0, -90, 0), item.transform);
        ParticleSystem.TrailModule settings = particle.GetComponent<ParticleSystem>().trails;
        settings.colorOverTrail = GetRarityColor(weapon.rarity);
    }

    public static void SpawnRandom(Vector3 position)
    {
        Debug.Log(AllItems[0]);
        Debug.Log(AllItems[1]);
        Debug.Log(AllItems[2]);
        object item = AllItems[Random.Range(0, AllItems.Length)];
        if (item is Weapon)
        {
            Spawn(item as Weapon, position);
        }
        else if(item is Armor)
        {
            Spawn(item as Armor, position);
        }
        else
        {
            Debug.Log("Cannot spawn: " + item);
        }     
    }

    public static void SpawnRandomWeapon(Vector3 position)
    {
        Weapon item = weapons[Random.Range(0, weapons.Length)];       
        Spawn(item, position);      
    }
    
    public static void SpawnRandomArmor(Vector3 position)
    {
        Armor item = armor[Random.Range(0, armor.Length)];
        Spawn(item, position);
    }

    public static Color GetRarityColor(object rarity)
    {
        if(rarity.ToString() == "Common")
        {
            return new Color(0.4f, 0.4f, 0.4f);
        }
        else if(rarity.ToString() == "Uncommon")
        {
            return new Color(0, 0.4f, 0);
        }
        else if (rarity.ToString() == "Rare")
        {
            return new Color(0, 0.35f, 1);
        }
        else if (rarity.ToString() == "Epic")
        {
            return new Color(0.65f, 0, 1);
        }
        else if (rarity.ToString() == "Legendary")
        {
            return new Color(1, 0.4f, 0);
        }
        else
        {
            return new Color(0, 0, 0);
        }
    }
}
