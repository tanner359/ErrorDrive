using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Equiper : MonoBehaviour
{
    private Mesh originalMesh;
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
                    bodyParts[i].GetComponent<SpriteRenderer>().sprite = gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite;                    
                }
                equipped = true;
                if(itemEquipped.tag == "Head")
                {
                    playerInventory.head = itemEquipped;
                }
                else if (itemEquipped.tag == "Chest")
                {
                    playerInventory.chest = itemEquipped;
                }
                else if (itemEquipped.tag == "Arms")
                {
                    playerInventory.arms = itemEquipped;
                }
                else if(itemEquipped.tag == "Boots")
                {
                    playerInventory.boots = itemEquipped;
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
                //itemEquipped.GetComponent<SpriteRenderer>().sortingOrder = 0;
                itemEquipped.GetComponent<MeshCollider>().isTrigger = true;
                equipped = true;
                itemEquipped.transform.GetChild(0).gameObject.SetActive(false);
                if (itemEquipped.tag == "Main Hand")
                {
                    playerInventory.mainHand = itemEquipped;
                }
                else if (itemEquipped.tag == "Off Hand")
                {
                    playerInventory.offHand = itemEquipped;
                }
            }
        }
        if(gameObject.transform.childCount == 1 && equipped && itemEquipped.tag != "Main Hand" && itemEquipped.tag != "Off Hand")
        {
            for (int i = 0; i < bodyParts.Count; i++)
            {
                bodyParts[i].GetComponent<MeshFilter>().mesh = originalMesh;
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
