using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.IO;

public class InventoryManager : MonoBehaviour
{

    GameObject baseIcon;
    public Inventory inventory;
    //public List<GameObject> slots;  
    public Item itemHolding;
    Slot originSlot;
    Slot currentSlot;
    public Transform inventorySlots;
    public Transform equipmentSlots;
    bool inventoryOpen;
    GameObject icon;

    GameObject player;

    Player_Inputs playerInputs;
    Vector2 mousePosition;

    private void OnEnable()
    {
        baseIcon = Resources.Load<GameObject>(Path.Combine("Prefabs", "BaseIcon"));

        if (playerInputs == null)
        {
            playerInputs = new Player_Inputs();
        }

        playerInputs.Player.Enable();
    }

    private void Start()
    {
        //for(int i = 0; i < inventorySlots.childCount; i++)
        //{
        //    slots.Add(inventorySlots.GetChild(i).gameObject);
        //}
        player = Combat.player;      
    }




    private void Update()
    {
        mousePosition = new Vector3(playerInputs.Player.MousePosition.ReadValue<Vector2>().x, playerInputs.Player.MousePosition.ReadValue<Vector2>().y, Mathf.Abs(Camera.main.transform.position.z));
        if (itemHolding != null)
        {
            currentSlot = FindSlot().GetComponent<Slot>();
            icon.transform.position = mousePosition;           
        }
    }

    public void OnRightMouseButton(InputValue value)
    {
        float clickValue = value.Get<float>();

        if(clickValue == 1) // button down
        {
            if (IsSlotAvailable(currentSlot) && itemHolding != null) //place item if slot available
            {
                PlaceItem(currentSlot);
            }
            else if (!IsSlotAvailable(currentSlot) && itemHolding != null) //drop item if slot is not available and holding item
            {
                DropItem(itemHolding);
            }
            else if (!IsSlotAvailable(currentSlot) && itemHolding == null) //move item if slot has an item and not holding an item
            {
                MoveItem(itemHolding);
            }
        }          
    }

    public void PlaceItem(Slot slot)
    {
        Slot.TransferItem(originSlot, slot);
    }

    public void DropItem(Item item)
    {
        ItemSystem.Spawn(item, player.transform.position + player.transform.forward * 3 + Vector3.up * 3);
        inventory.inventoryList.Remove(item);
    }

    public void MoveItem(Item item)
    {
        icon = Instantiate(baseIcon, mousePosition, Quaternion.identity, transform);
        icon.GetComponent<Image>().sprite = item.sprite;
    }

    public bool IsSlotAvailable(Slot slot)
    {
        if(slot != null){return true;}
        else{return false;}       
    }

    


    //public void updateInventory(List<GameObject> inventoryItems)
    //{
    //    Debug.Log("update inventory");
    //    for (int k = 0; k < slots.Count; k++)
    //    {
    //        if (slots[k].transform.childCount == 1)
    //        {
    //            inventoryItems[inventoryItems.Count-1].transform.parent = slots[k].transform;
    //            inventoryItems[inventoryItems.Count-1].SetActive(false);
    //            slots[k].transform.GetChild(0).GetComponent<Image>().sprite = inventoryItems[inventoryItems.Count-1].GetComponent<Stats>().source.sprite;
    //            slots[k].transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().color = ItemSystem.GetRarityColor(inventoryItems[inventoryItems.Count - 1].GetComponent<Stats>().source.rarity);
    //            slots[k].transform.GetChild(0).GetComponentInChildren<Text>().text = inventoryItems[inventoryItems.Count-1].name;
    //            slots[k].transform.GetChild(0).GetComponent<Image>().color = new Color(slots[k].transform.GetChild(0).GetComponent<Image>().color.r, slots[k].transform.GetChild(0).GetComponent<Image>().color.g, slots[k].transform.GetChild(0).GetComponent<Image>().color.b, 100);
    //            k = 9999;
    //        }
    //    }
    //}
    //public void moveObject(GameObject slot)
    //{
    //    Debug.Log("move object");
    //    if (!isHoldingItem && slot.transform.childCount > 1)
    //    {
    //        slot.transform.GetChild(0).GetComponent<Image>().sprite = null;
    //        slot.transform.GetChild(0).GetComponent<Image>().color = new Color(slot.transform.GetChild(0).GetComponent<Image>().color.r, slot.transform.GetChild(0).GetComponent<Image>().color.g, slot.transform.GetChild(0).GetComponent<Image>().color.b, 0);
    //        slot.transform.GetChild(0).GetComponentInChildren<Text>().text = "";
    //        itemHolding = slot.transform.GetChild(1).gameObject;
    //        itemHolding.transform.parent = null;
    //        //itemHolding.GetComponent<SpriteRenderer>().sortingOrder = 10;
    //        isHoldingItem = true;
    //        itemHolding.gameObject.SetActive(true);
    //        itemHolding.GetComponent<Rigidbody>().isKinematic = true;
    //        itemHolding.transform.eulerAngles = new Vector3(0, 0, 0);       
    //        if (slot.transform.GetChild(0).transform.GetChild(0).transform.childCount > 0)
    //        {
    //            slot.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
    //        }
    //    }
    //}
    //public void placeObject()
    //{
    //    Debug.Log("place item");
    //    if (FindSlot().gameObject.CompareTag("Inventory Slot"))
    //    {
    //        slot.transform.GetChild(0).GetComponent<Button>().enabled = false;
    //        itemHolding.transform.parent = slot.transform;
    //        itemHolding.SetActive(false);
    //        slot.transform.GetChild(0).GetComponent<Image>().sprite = itemHolding.GetComponent<Stats>().source.sprite;
    //        slot.transform.GetChild(0).GetComponentInChildren<Text>().text = itemHolding.name;
    //        slot.transform.GetChild(0).transform.GetChild(0).GetComponentInChildren<Text>().color = ItemSystem.GetRarityColor(itemHolding.GetComponent<Stats>().source.rarity);
    //        slot.transform.GetChild(0).GetComponent<Image>().color = new Color(slot.transform.GetChild(0).GetComponent<Image>().color.r, slot.transform.GetChild(0).GetComponent<Image>().color.g, slot.transform.GetChild(0).GetComponent<Image>().color.b, 100);
    //        isHoldingItem = false;
    //        itemHolding = null;
    //    }
    //    else if (FindSlot().gameObject.CompareTag(itemHolding.gameObject.tag))
    //    {
    //        slot.transform.GetChild(0).GetComponent<Button>().enabled = false;
    //        slot.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
    //        itemHolding.transform.parent = slot.transform;
    //        itemHolding.SetActive(false);
    //        slot.transform.GetChild(0).GetComponent<Image>().sprite = itemHolding.GetComponent<Stats>().source.sprite;
    //        slot.transform.GetChild(0).transform.GetChild(0).GetComponentInChildren<Text>().color = ItemSystem.GetRarityColor(itemHolding.GetComponent<Stats>().source.rarity);
    //        slot.transform.GetChild(0).GetComponentInChildren<Text>().text = itemHolding.name;
    //        slot.transform.GetChild(0).GetComponent<Image>().color = new Color(slot.transform.GetChild(0).GetComponent<Image>().color.r, slot.transform.GetChild(0).GetComponent<Image>().color.g, slot.transform.GetChild(0).GetComponent<Image>().color.b, 100);
    //        isHoldingItem = false;
    //        itemHolding = null;
    //    }
        
    //}
    //public void dropObject()
    //{
    //    Debug.Log("drop item");
    //    inventory.inventoryList.Remove(itemHolding);
    //    itemHolding.SetActive(true);
    //    itemHolding.GetComponent<Rigidbody>().isKinematic = false;
    //    itemHolding.transform.parent = null;
    //    //itemHolding.GetComponent<SpriteRenderer>().sortingOrder = 0;
    //    itemHolding.transform.position = player.transform.position + player.transform.forward + new Vector3(0,3,0);
    //    isHoldingItem = false;
    //    itemHolding = null;
    //}

    public Slot FindSlot()
    {       
        Slot slot = null;
        RaycastHit hit;
        Physics.Raycast(mousePosition, Vector3.forward, out hit);
        Debug.DrawRay(mousePosition, Vector3.forward, Color.red);
        slot = hit.collider.GetComponent<Slot>();
        return slot;        
    }  
}
