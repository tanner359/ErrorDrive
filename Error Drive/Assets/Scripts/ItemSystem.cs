using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class ItemSystem
{
    public static GameObject baseItem = Resources.Load<GameObject>(Path.Combine("Prefabs", "BaseItem"));
    public static Item[] AllItems = Resources.LoadAll<Item>("Items");   
    public static GameObject rarityParticle = Resources.Load<GameObject>(Path.Combine("Particles", "Rarity_Particle"));
 
    public static void Spawn(Item item, Vector3 position)
    {
        GameObject newItem = Object.Instantiate(baseItem, position, Quaternion.identity);       
        ItemProperties props = new ItemProperties(newItem);

        newItem.name = item.itemName;
        newItem.tag = item.equipSlot.ToString();
        props.transform.position = position;
        props.meshFilter.mesh = item.mesh;
        props.collider.sharedMesh = item.mesh;
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
