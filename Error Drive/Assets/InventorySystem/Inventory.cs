using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    Stats stats;
    InventoryManager inventoryManager;

    public List<Item> inventoryList = new List<Item>();
    public Collider[] interactableItems;

    public SphereCollider playerZone;
    public LayerMask contactFilter;
    public Item Main_Hand, Off_Hand, Head, Body, Left_Leg, Right_Leg;

    public Animator animator;
    
    public int inventoryMax = 20;

    bool inventoryOpen;



    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<Stats>();
        inventoryManager = GameObject.Find("Canvas_Display").transform.GetChild(0).GetComponent<InventoryManager>();
    }


    // Update is called once per frame
    private void Update()
    {
        ScanInteractArea(playerZone);
        UpdateStats();
    }

    Item[] equipped = new Item[6];
    public void UpdateStats()
    {
        if (equipped[0] != Main_Hand)
        {
            equipped[0] = Main_Hand;
            stats.AddStats(Main_Hand);
        }
        else if(equipped[1] != Off_Hand)
        {
            equipped[1] = Off_Hand;
            stats.AddStats(Off_Hand);
        }
        else if (equipped[2] != Head)
        {
            equipped[2] = Head;
            stats.AddStats(Head);
        }
        else if (equipped[3] != Body)
        {
            equipped[3] = Body;
            stats.AddStats(Body);
        }
        else if (equipped[4] != Left_Leg)
        {
            equipped[4] = Left_Leg;
            stats.AddStats(Left_Leg);
        }
        else if (equipped[5] != Right_Leg)
        {
            equipped[5] = Right_Leg;
            stats.AddStats(Right_Leg);
        }
        else
        {
            return;
        }

    }


    public void OnInventory()
    {
        InventoryOpenClose();
    }

    public void OnPickup()
    {
        if (interactableItems.Length > 0){ 
            PickUp();
            StartCoroutine(StatusEffects.SlowTarget(gameObject, 0.60f, 0.5f));
            #region Animation
            animator.SetTrigger("Pickup");
            #endregion
        }
    }

    public void OnDrop()
    {
        DropItem(inventoryList[0]);
        #region Animation
        //animator.SetBool("Drop", true);
        #endregion
    }



    public void InventoryOpenClose()
    {
        if (!inventoryOpen)
        {
            PlayerSettings.DisableControl();
            inventoryOpen = true;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            inventoryManager.gameObject.SetActive(true);
        }
        else
        {
            PlayerSettings.EnableControl();
            inventoryOpen = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            inventoryManager.gameObject.SetActive(false);
        }
    }

    public void PickUp(){ // pick up closest item
        if(inventoryList.Count != 20)
        {
            Item item = GetClosestItem(transform.position, interactableItems).GetComponent<Stats>().source;            
            inventoryList.Add(item);
            //inventoryManager.updateInventory(inventoryList);
        }
        else
        {
            Debug.Log("Inventory Full");
        }
    }
    public void DropItem(Item item){

        ItemSystem.Spawn(item, transform.position + transform.forward * 3 + Vector3.up * 3);       
        inventoryList.RemoveAt(0);
    }

    public void ScanInteractArea(SphereCollider playerCollider)
    { // check for interactables  
        interactableItems = Physics.OverlapSphere(transform.position + new Vector3(0, playerZone.center.y, 0) , playerCollider.radius, contactFilter);
        

        if (interactableItems.Length > 0)
        {
            CanvasDisplay.DisplayInteractText(GetClosestItem(transform.position, interactableItems).transform.position, "E");
        }
        else
        {
            CanvasDisplay.HideText();
        }
    }

    public GameObject GetClosestItem(Vector3 playerPos, Collider[] interactableItems){ // returns back the closest item
        GameObject closestItem = interactableItems[0].gameObject;
        float minDistance = Vector3.Distance(playerPos, interactableItems[0].gameObject.transform.position);
        for (int i = 0; i < interactableItems.Length; i++){
            if(minDistance > Vector3.Distance(playerPos, interactableItems[i].gameObject.transform.position)){
                minDistance = Vector3.Distance(playerPos, interactableItems[i].gameObject.transform.position);
                closestItem = interactableItems[i].gameObject;
            }
        }      
        return closestItem;
    }
}
