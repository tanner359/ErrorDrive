using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equip_Slot : MonoBehaviour
{
    private List<Mesh> originalMeshes = new List<Mesh>();
    private List<Material> originalMaterials = new List<Material>();
    public List<GameObject> bodyParts;

    public Mesh GetOriginalMeshes(int index){return originalMeshes[index];}
    public Material GetOriginalMaterials(int index){return originalMaterials[index];}

    void Start()
    {
        for(int i = 0; i < bodyParts.Count; i++)
        {
            originalMeshes.Add(bodyParts[i].GetComponent<MeshFilter>().mesh);
            originalMaterials.Add(bodyParts[i].GetComponent<MeshRenderer>().material);
        }
    }      
}
