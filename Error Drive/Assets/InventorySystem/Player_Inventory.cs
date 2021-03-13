using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Inventory : MonoBehaviour
{
    CanvasDisplay canvasDisplay;

    InventoryManager inventoryManager;

    public List<GameObject> inventoryList = new List<GameObject>();
    public Collider[] interactableItems;

    public SphereCollider playerZone;
    public LayerMask contactFilter;
    public GameObject Main_Hand, Off_Hand, Head, Body, Left_Leg, Right_Leg;

    public Animator animator;
    

    public int inventoryMax = 20;

    bool inventoryOpen;



    // Start is called before the first frame update
    void Start()
    {
        canvasDisplay = GameObject.Find("Canvas_Display").GetComponent<CanvasDisplay>();
        inventoryManager = GameObject.Find("Canvas_Display").transform.GetChild(0).GetComponent<InventoryManager>();
    }


    // Update is called once per frame
    private void Update()
    {
        checkForInteractables(playerZone);
    }


    public void OnInventory()
    {
        InventoryOpenClose();
    }

    public void OnPickup()
    {
        if (interactableItems.Length > 0){ 
            pickUp();
            #region Animation
            animator.SetTrigger("Pickup");
            #endregion
        }
    }

    public void OnDrop()
    {
        DropItem(inventoryList[0].gameObject);
        #region Animation
        //animator.SetBool("Drop", true);
        #endregion
    }



    public void InventoryOpenClose()
    {
        if (!inventoryOpen)
        {
            gameObject.GetComponent<Player_Controller>().SetControl(false);
            inventoryOpen = true;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            inventoryManager.gameObject.SetActive(true);
        }
        else
        {
            gameObject.GetComponent<Player_Controller>().SetControl(true);
            inventoryOpen = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            inventoryManager.gameObject.SetActive(false);
        }
    }

    public void pickUp(){ // pick up closest item
        if(inventoryList.Count != 20)
        {
            inventoryList.Add(getClosestItem(gameObject.transform, interactableItems));
            inventoryManager.updateInventory(inventoryList);
        }
        else
        {
            Debug.Log("Inventory Full");
        }
    }
    public void DropItem(GameObject itemToDrop){
        itemToDrop.AddComponent<Rigidbody>();
        itemToDrop.GetComponent<Collider>().isTrigger = false;
        itemToDrop.GetComponent<Transform>().SetParent(null);
        inventoryList.RemoveAt(0);
    }

    public void checkForInteractables(SphereCollider playerCollider)
    { // check for interactables       
        interactableItems = Physics.OverlapSphere(transform.position + new Vector3(0, playerZone.center.y, 0) , playerCollider.radius, contactFilter);
        

        if (interactableItems.Length > 0)
        {
            canvasDisplay.displayInteractText(getClosestItem(gameObject.transform, interactableItems).transform, "E");
        }
        else
        {
            canvasDisplay.HideText();
        }
    }

    public GameObject getClosestItem(Transform playerPos, Collider[] interactableItems){ // returns back the closest item
        GameObject closestItem = interactableItems[0].gameObject;
        float minDistance = Vector3.Distance(playerPos.position, interactableItems[0].gameObject.transform.position);
        for (int i = 0; i < interactableItems.Length; i++){
            if(minDistance > Vector3.Distance(playerPos.position, interactableItems[i].gameObject.transform.position)){
                minDistance = Vector3.Distance(playerPos.position, interactableItems[i].gameObject.transform.position);
                closestItem = interactableItems[i].gameObject;
            }
        }
        return closestItem;
    }
}
