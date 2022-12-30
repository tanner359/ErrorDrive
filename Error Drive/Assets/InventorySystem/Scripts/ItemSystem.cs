using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class ItemSystem
{
    public static GameObject baseItem = Resources.Load<GameObject>(Path.Combine("Prefabs", "BaseItem"));
    public static Item[] AllItems = Resources.LoadAll<Item>("Items");
    public static Item[] CommonItems = Resources.LoadAll<Item>(Path.Combine("Items", "Common"));
    public static Item[] UnCommonItems = Resources.LoadAll<Item>(Path.Combine("Items", "UnCommon"));
    public static Item[] RareItems = Resources.LoadAll<Item>(Path.Combine("Items", "Rare"));
    public static Item[] EpicItems = Resources.LoadAll<Item>(Path.Combine("Items", "Epic"));
    public static Item[] LegendaryItems = Resources.LoadAll<Item>(Path.Combine("Items", "Legendary"));
    public static GameObject rarityParticle = Resources.Load<GameObject>(Path.Combine("Particles", "Rarity_Beam"));
 
    public static void Spawn(Item item, Vector3 position)
    {
        GameObject newItem = Object.Instantiate(baseItem, position, Quaternion.identity);       
        ItemProperties props = new ItemProperties(newItem);

        newItem.name = item.itemName;
        newItem.tag = item.equipSlot.ToString();
        props.transform.position = position;
        props.meshFilter.mesh = item.meshes[0];
        props.collider.sharedMesh = item.meshes[0];
        props.meshRend.material = item.material;
        props.stats.SetStats(item);

        newItem.GetComponent<Transform>().localScale = newItem.GetComponent<Transform>().localScale / 2.5f;

        GameObject particle = Object.Instantiate(rarityParticle, newItem.transform.position, Quaternion.Euler(0, -90, 0), newItem.transform);
        ParticleSystem.TrailModule settings = particle.GetComponent<ParticleSystem>().trails;
        settings.colorOverTrail = GetRarityColor(item.rarity);
    }

    public static void SpawnRandom(Vector3 position)
    {      
        Item item = AllItems[Random.Range(0, AllItems.Length)];
        if(item == null){
            Debug.Log("No Items Available to Spawn");
            return;
        }
        Spawn(item, position);
    }

    public static void CalculateDrops(Vector3 position)
    {
        float i = Random.Range(0.0f, 100.0f);

        if (i <= 1.5f)
        {
            Item item = LegendaryItems[Random.Range(0, LegendaryItems.Length)];
            Spawn(item, position);
            return;
        }
        else if (i <= 3.0f)
        {
            Item item = EpicItems[Random.Range(0, EpicItems.Length)];
            Spawn(item, position);
            return;
        }
        else if (i <= 6.25f)
        {
            Item item = RareItems[Random.Range(0, RareItems.Length)];
            Spawn(item, position);
            return;
        }
        else if (i <= 12.5f)
        {
            Item item = UnCommonItems[Random.Range(0, UnCommonItems.Length)];
            Spawn(item, position);
            return;
        }
        else if (i <= 25f)
        {
            Item item = CommonItems[Random.Range(0, CommonItems.Length)];
            Spawn(item, position);
            return;
        }
        return;
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
