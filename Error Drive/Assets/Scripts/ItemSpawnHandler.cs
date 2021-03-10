using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnHandler : MonoBehaviour
{
    public List<Weapon> weapons;
    public List<Armor> armor;
    public GameObject itemParticle;
    

    float offset;

    // Start is called before the first frame update
    void Start()
    {
        GameObject item = new GameObject(weapons[0].weaponName);
        item.tag = weapons[0].tag;
        item.layer = 9;
        item.GetComponent<Transform>().position = GameObject.Find("Player").transform.position + new Vector3(offset += 2, 3, 0);
        item.GetComponent<Transform>().localScale = item.GetComponent<Transform>().localScale / 2f;
        item.AddComponent<Rigidbody>();
        item.AddComponent<MeshFilter>().mesh = weapons[0].Model;
        item.AddComponent<MeshCollider>().convex = true;
        item.AddComponent<MeshRenderer>().material = weapons[0].material;
        item.AddComponent<Item_Stats>();
        item.GetComponent<Item_Stats>().weapon = weapons[0];
        GameObject particle = Instantiate(itemParticle, item.transform.position, Quaternion.Euler(0, 0, 0), item.transform);
        ParticleSystem.MainModule settings = particle.GetComponent<ParticleSystem>().main;
        settings.startColor = weapons[0].rarityColor;
        
    }

    public void spawnItem(Armor armor, Weapon weapon, Transform location)
    {
        if (armor)
        {
            GameObject item = new GameObject(armor.armorName);
            item.tag = armor.tag;
            item.layer = 9;
            item.GetComponent<Transform>().position = location.position;
            item.GetComponent<Transform>().localScale = item.GetComponent<Transform>().localScale / 4f;
            item.AddComponent<Rigidbody2D>();
            item.AddComponent<SpriteRenderer>().sprite = armor.armorSprite;
            item.AddComponent<PolygonCollider2D>();
            item.GetComponent<SpriteRenderer>().material = armor.armorMaterial;
            item.AddComponent<Item_Stats>();
            item.GetComponent<Item_Stats>().armor = armor;
            GameObject particle = Instantiate(itemParticle, item.transform.position, Quaternion.Euler(0, -90, 0), item.transform);
            ParticleSystem.MainModule settings = particle.GetComponent<ParticleSystem>().main;
            settings.startColor = armor.rarityColor;
        }
        if (weapon)
        {
            GameObject item = new GameObject(weapon.weaponName);
            item.tag = weapon.tag;
            item.layer = 9;
            item.GetComponent<Transform>().position = location.position;
            item.GetComponent<Transform>().localScale = item.GetComponent<Transform>().localScale / 2f;
            item.AddComponent<Rigidbody2D>();
            item.AddComponent<SpriteRenderer>().sprite = weapon.weaponSprite;
            item.AddComponent<PolygonCollider2D>();
            item.GetComponent<SpriteRenderer>().material = weapon.material;
            item.AddComponent<Item_Stats>();
            item.GetComponent<Item_Stats>().weapon = weapon;
            GameObject particle = Instantiate(itemParticle, item.transform.position, Quaternion.Euler(0, -90, 0), item.transform);
            ParticleSystem.MainModule settings = particle.GetComponent<ParticleSystem>().main;
            settings.startColor = weapon.rarityColor;
        }
        
    }
}
