using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Equiper : MonoBehaviour
{
    private Mesh originalMesh;
    private Material originalMaterial;
    public List<GameObject> bodyParts;
    public bool equipped = false;
    public Inventory inventory;

    public GameObject itemEquipped;
    // Start is called before the first frame update
    void Start()
    {
        if (bodyParts[0].name != "Hand_01" && bodyParts[0].name != "Hand_02")
        {
            originalMesh = bodyParts[0].GetComponent<MeshFilter>().mesh;
            originalMaterial = bodyParts[0].GetComponent<MeshRenderer>().material;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.childCount > 1 && !equipped)
        {
            itemEquipped = gameObject.transform.GetChild(1).gameObject;
            for (int i = 0; i < bodyParts.Count; i++)
            {
                bodyParts[i].GetComponent<MeshFilter>().mesh = gameObject.transform.GetChild(1).GetComponent<MeshFilter>().mesh;
                bodyParts[i].GetComponent<MeshRenderer>().material = gameObject.transform.GetChild(1).GetComponent<MeshRenderer>().material;

                if (itemEquipped.tag == "Main_Hand")
                {
                    inventory.Main_Hand = itemEquipped;
                    bodyParts[i].GetComponent<MeshCollider>().enabled = true;
                    bodyParts[i].GetComponent<MeshCollider>().sharedMesh = gameObject.transform.GetChild(1).GetComponent<MeshFilter>().mesh;
                }
                else if (itemEquipped.tag == "Off_Hand")
                {
                    inventory.Off_Hand = itemEquipped;
                    bodyParts[i].GetComponent<MeshCollider>().enabled = true;
                    bodyParts[i].GetComponent<MeshCollider>().sharedMesh = gameObject.transform.GetChild(1).GetComponent<MeshFilter>().mesh;
                }
            }
            equipped = true;          
            if (itemEquipped.tag == "Head")
            {
                inventory.Head = itemEquipped;
            }
            else if (itemEquipped.tag == "Body")
            {
                inventory.Body = itemEquipped;
            }
            else if (itemEquipped.tag == "Left_Leg")
            {
                inventory.Left_Leg = itemEquipped;
            }
            else if (itemEquipped.tag == "Right_Leg")
            {
                inventory.Right_Leg = itemEquipped;
            }          
        }
        if(gameObject.transform.childCount == 1 && equipped && itemEquipped.tag != "Main_Hand" && itemEquipped.tag != "Off_Hand")
        {
            for (int i = 0; i < bodyParts.Count; i++)
            {
                bodyParts[i].GetComponent<MeshFilter>().mesh = originalMesh;
                bodyParts[i].GetComponent<MeshRenderer>().material = originalMaterial;
            }
            equipped = false;
        }
        else if (gameObject.transform.childCount == 1 && equipped)
        {
            Destroy(itemEquipped);
            equipped = false;
        }
    }

    public int getDirection()
    {
        if (GameObject.Find("Player").transform.localScale.x > 0)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }
}
