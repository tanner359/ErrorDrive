using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentLink : MonoBehaviour
{
    public Dictionary<Item.EquipSlot, List<GameObject>> bodyLinks;
    public Dictionary<Item.EquipSlot, List<GameObject>> ignoreLinks;
    public Dictionary<Item.EquipSlot, List<Mesh>> originalMeshes;
    public Dictionary<Item.EquipSlot, List<Material>> originalMaterials;

    [Header("Main Hand")]
    public List<GameObject> MainHand_meshes;
    public List<GameObject> MainHand_ignores;

    [Header("Off Hand")]
    public List<GameObject> OffHand_meshes;
    public List<GameObject> OffHand_ignores;

    [Header("Torso")]
    public List<GameObject> Torso_meshes;
    public List<GameObject> Torso_ignores;

    [Header("Head")]
    public List<GameObject> Head_meshes;
    public List<GameObject> Head_ignores;

    [Header("L_Leg")]
    public List<GameObject> L_Leg_meshes;
    public List<GameObject> L_Leg_ignores;

    [Header("R_Leg")]
    public List<GameObject> R_Leg_meshes;
    public List<GameObject> R_Leg_ignores;

    public Mesh GetOriginalMeshes(Item.EquipSlot equipSlot, int index)
    {
        originalMeshes.TryGetValue(equipSlot, out List<Mesh> meshes);
        return meshes[index];
    }
    public Material GetOriginalMaterials(Item.EquipSlot equipSlot, int index)
    {
        originalMaterials.TryGetValue(equipSlot, out List<Material> materials);
        return materials[index];
    }  
    public List<GameObject> GetEquipLinks(Item.EquipSlot equipSlot)
    {
        bodyLinks.TryGetValue(equipSlot, out List<GameObject> links);
        return links;
    }
    public void IgnorePartsSetActive(Item.EquipSlot equipSlot, bool state)
    {
        ignoreLinks.TryGetValue(equipSlot, out List<GameObject> ignoreParts);

        if (ignoreParts.Count > 0)
        {
            for (int i = 0; i < ignoreParts.Count; i++)
            {
                ignoreParts[i].SetActive(state);
            }
        }
        return;
    }
    void Start() // sets the material and mesh defaults for each equip slot, could be simplified more
    {
        List<Mesh> meshes = new List<Mesh>();
        List<Material> materials = new List<Material>();
        
        for (int i = 0; i < MainHand_meshes.Count-1; i++)
        {
            meshes.Add(MainHand_meshes[i].GetComponent<MeshFilter>().mesh);
            materials.Add(MainHand_meshes[i].GetComponent<MeshRenderer>().material);
        }
        if (meshes.Count > 0)
        {
            originalMeshes.Add(Item.EquipSlot.Main_Hand, meshes);
            originalMaterials.Add(Item.EquipSlot.Main_Hand, materials);
            bodyLinks.Add(Item.EquipSlot.Main_Hand, MainHand_meshes);
            ignoreLinks.Add(Item.EquipSlot.Main_Hand, MainHand_ignores);
        }
        meshes = new List<Mesh>();
        materials = new List<Material>();

        for (int i = 0; i < OffHand_meshes.Count-1; i++)
        {
            meshes.Add(OffHand_meshes[i].GetComponent<MeshFilter>().mesh);
            materials.Add(OffHand_meshes[i].GetComponent<MeshRenderer>().material);
        }
        if (meshes.Count > 0)
        {
            originalMeshes.Add(Item.EquipSlot.Off_Hand, meshes);
            originalMaterials.Add(Item.EquipSlot.Off_Hand, materials);
            bodyLinks.Add(Item.EquipSlot.Off_Hand, OffHand_meshes);
            ignoreLinks.Add(Item.EquipSlot.Off_Hand, OffHand_ignores);
        }
        meshes = new List<Mesh>();
        materials = new List<Material>();

        for (int i = 0; i < Torso_meshes.Count-1; i++)
        {
            meshes.Add(Torso_meshes[i].GetComponent<MeshFilter>().mesh);
            materials.Add(Torso_meshes[i].GetComponent<MeshRenderer>().material);
        }
        if (meshes.Count > 0)
        {
            originalMeshes.Add(Item.EquipSlot.Torso, meshes);
            originalMaterials.Add(Item.EquipSlot.Torso, materials);
            bodyLinks.Add(Item.EquipSlot.Torso, Torso_meshes);
            ignoreLinks.Add(Item.EquipSlot.Torso, Torso_ignores);
        }
        meshes = new List<Mesh>();
        materials = new List<Material>();

        for (int i = 0; i < Head_meshes.Count-1; i++)
        {
            meshes.Add(Head_meshes[i].GetComponent<MeshFilter>().mesh);
            materials.Add(Head_meshes[i].GetComponent<MeshRenderer>().material);
        }
        if (meshes.Count > 0)
        {
            originalMeshes.Add(Item.EquipSlot.Head, meshes);
            originalMaterials.Add(Item.EquipSlot.Head, materials);
            bodyLinks.Add(Item.EquipSlot.Head, Head_meshes);
            ignoreLinks.Add(Item.EquipSlot.Head, Head_ignores);
        }
        meshes = new List<Mesh>();
        materials = new List<Material>();

        for (int i = 0; i <= L_Leg_meshes.Count-1; i++)
        {
            meshes.Add(L_Leg_meshes[i].GetComponent<MeshFilter>().mesh);
            materials.Add(L_Leg_meshes[i].GetComponent<MeshRenderer>().material);
        }
        if (meshes.Count > 0)
        {
            Debug.Log(meshes.Count);
            originalMeshes.Add(Item.EquipSlot.L_Leg, meshes);
            originalMaterials.Add(Item.EquipSlot.L_Leg, materials);
            bodyLinks.Add(Item.EquipSlot.L_Leg, L_Leg_meshes);
            ignoreLinks.Add(Item.EquipSlot.L_Leg, L_Leg_ignores);
        }
        meshes = new List<Mesh>();
        materials = new List<Material>();

        for (int i = 0; i <= R_Leg_meshes.Count-1; i++)
        {
            meshes.Add(R_Leg_meshes[i].GetComponent<MeshFilter>().mesh);
            materials.Add(R_Leg_meshes[i].GetComponent<MeshRenderer>().material);
        }
        if (meshes.Count > 0)
        {
            originalMeshes.Add(Item.EquipSlot.R_Leg, meshes);
            originalMaterials.Add(Item.EquipSlot.R_Leg, materials);
            bodyLinks.Add(Item.EquipSlot.R_Leg, R_Leg_meshes);
            ignoreLinks.Add(Item.EquipSlot.R_Leg, R_Leg_ignores);
        }
        meshes = new List<Mesh>();
        materials = new List<Material>();
    } 
}
