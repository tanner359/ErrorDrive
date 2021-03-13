using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Equiper : MonoBehaviour
{
    private Mesh originalMesh;
    private Material originalMaterial;
    public List<GameObject> bodyParts;
    public bool equipped = false;
    public Player_Inventory playerInventory;

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
            if (itemEquipped.tag != "Main Hand" && itemEquipped.tag != "Off Hand") // if not a weapon
            {
                for(int i = 0; i < bodyParts.Count; i++)
                {
                    bodyParts[i].GetComponent<MeshFilter>().mesh = gameObject.transform.GetChild(1).GetComponent<MeshFilter>().mesh;
                    bodyParts[i].GetComponent<MeshRenderer>().material = gameObject.transform.GetChild(1).GetComponent<MeshRenderer>().material;
                }
                equipped = true;
                if(itemEquipped.tag == "Head")
                {
                    playerInventory.Head = itemEquipped;
                }
                else if (itemEquipped.tag == "Body")
                {
                    playerInventory.Body = itemEquipped;
                }
                else if (itemEquipped.tag == "Left_Leg")
                {
                    playerInventory.Left_Leg = itemEquipped;
                }
                else if (itemEquipped.tag == "Right_Leg")
                {
                    playerInventory.Right_Leg = itemEquipped;
                }              
            }
            else //if a weapon main or off hand
            {
                GameObject weaponCopy = Instantiate(itemEquipped);
                weaponCopy.transform.localScale = itemEquipped.transform.lossyScale;
                itemEquipped = weaponCopy;
                itemEquipped.transform.SetParent(bodyParts[0].transform);
                itemEquipped.transform.position = bodyParts[0].transform.position;
                itemEquipped.SetActive(true);
                itemEquipped.transform.eulerAngles = bodyParts[0].transform.eulerAngles + new Vector3(0,0,60 * getDirection());              
                itemEquipped.GetComponent<Rigidbody>().isKinematic = true;
                itemEquipped.GetComponent<MeshCollider>().isTrigger = true;
                equipped = true;
                itemEquipped.transform.GetChild(0).gameObject.SetActive(false);
                if (itemEquipped.tag == "Main_Hand")
                {
                    playerInventory.Main_Hand = itemEquipped;
                }
                else if (itemEquipped.tag == "Off_Hand")
                {
                    playerInventory.Off_Hand = itemEquipped;
                }
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
