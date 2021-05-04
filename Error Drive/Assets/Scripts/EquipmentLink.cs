using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentLink : MonoBehaviour
{
    #region DICTIONARIES

    public Dictionary<Item.EquipSlot, List<GameObject>> bodyLinks = new Dictionary<Item.EquipSlot, List<GameObject>>();
    public Dictionary<Item.EquipSlot, List<GameObject>> ignoreLinks = new Dictionary<Item.EquipSlot, List<GameObject>>();
    public Dictionary<Item.EquipSlot, List<Mesh>> originalMeshes = new Dictionary<Item.EquipSlot, List<Mesh>>();
    public Dictionary<Item.EquipSlot, List<Material>> originalMaterials = new Dictionary<Item.EquipSlot, List<Material>>();

    #endregion

    #region MESHES

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

    #endregion

    #region FUNCTIONS
    public Mesh GetOriginalMeshes(Item.EquipSlot equipSlot, int index) // returns the original mesh of the given equipSlot;
    {
        originalMeshes.TryGetValue(equipSlot, out List<Mesh> meshes);
        return meshes[index];
    }
    public Material GetOriginalMaterials(Item.EquipSlot equipSlot, int index) // returns the original material of the given equipSlot;
    {
        originalMaterials.TryGetValue(equipSlot, out List<Material> materials);
        return materials[index];
    }  
    public List<GameObject> GetEquipLinks(Item.EquipSlot equipSlot) //returns a list of the available body links that a given equip slot can use;
    {
        bodyLinks.TryGetValue(equipSlot, out List<GameObject> links);
        return links;
    }
    public void IgnorePartsSetActive(Item.EquipSlot equipSlot, bool state) // when called will set the desired ignore parts to be disabled or enabled
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
    #endregion

    void Start() // sets the material and mesh defaults for each equip slot
    {
        List<GameObject>[] MeshLists = { MainHand_meshes, OffHand_meshes, Torso_meshes, Head_meshes, R_Leg_meshes, L_Leg_meshes };
        List<GameObject>[] IgnoreLists = { MainHand_ignores, OffHand_ignores, Torso_ignores, Head_ignores, R_Leg_ignores, L_Leg_ignores };

        for (int k = 0; k < MeshLists.Length; k++)
        {         
            List<Mesh> meshes = new List<Mesh>();
            List<Material> materials = new List<Material>();
          
            for (int i = 0; i < MeshLists[k].Count; i++)
            {
                meshes.Add(MeshLists[k][i].GetComponent<MeshFilter>().mesh);
                materials.Add(MeshLists[k][i].GetComponent<MeshRenderer>().material);
            }
            Item.EquipSlot equipSlot = Item.EquipSlot.Main_Hand + k;
            if (meshes.Count > 0)
            {
                originalMeshes.Add(equipSlot, meshes);
            }
            if (materials.Count > 0)
            {
                originalMaterials.Add(equipSlot, materials);
            }
            bodyLinks.Add(equipSlot, MeshLists[k]);
            ignoreLinks.Add(equipSlot, IgnoreLists[k]);
        }
    }
}
