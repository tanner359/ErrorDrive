using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equip : MonoBehaviour
{
    private List<Mesh> originalMeshes = new List<Mesh>();
    private List<Material> originalMaterials = new List<Material>();
    public List<GameObject> bodyParts;
    public List<GameObject> ignoreParts;

    public Mesh GetOriginalMeshes(int index){return originalMeshes[index];}
    public Material GetOriginalMaterials(int index){return originalMaterials[index];}

    public void IgnorePartsSetActive(bool state)
    {
        if(ignoreParts.Count > 0)
        {
            for (int i = 0; i < ignoreParts.Count; i++)
            {
                ignoreParts[i].SetActive(state);
            }
        }
        return;
    }
    void Start()
    {
        for(int i = 0; i < bodyParts.Count; i++)
        {
            originalMeshes.Add(bodyParts[i].GetComponent<MeshFilter>().mesh);
            originalMaterials.Add(bodyParts[i].GetComponent<MeshRenderer>().material);
        }
    }    
}
