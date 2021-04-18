using UnityEngine;
using System.IO;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    Animator animator;
    Stats stats;

    public Transform inventoryContainer;
    GameObject inventorySlotsContainer;

    public Collider[] interactableItems;
    public SphereCollider playerZone;
    public LayerMask contactFilter;

    public int inventoryCount = 0;
    int inventoryMax = 20;

    bool isOpen;

    GameObject icon;
    public Item itemHolding;

    Player_Inputs playerInputs;
    Vector2 mousePosition;

    private void OnEnable()
    {
        if (playerInputs == null)
        {
            playerInputs = new Player_Inputs();
        }
        playerInputs.Player.Enable();
    }

    private void Awake()
    {
        if(icon == null) //spawns icon into the inventory disabled at awake
        {
            this.icon = Resources.Load<GameObject>(Path.Combine("Prefabs", "BaseIcon"));
            GameObject newIcon = Instantiate(icon, inventoryContainer);
            icon = newIcon;
            icon.SetActive(false);
        }
    }

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        stats = GetComponent<Stats>();

        inventorySlotsContainer = inventoryContainer.Find("InventorySlots").gameObject;
        inventoryMax = inventorySlotsContainer.transform.childCount;
    }


    // Update is called once per frame
    private void Update()
    {
        ScanInteractArea(playerZone);

        mousePosition = new Vector3(playerInputs.Player.MousePosition.ReadValue<Vector2>().x, playerInputs.Player.MousePosition.ReadValue<Vector2>().y, Mathf.Abs(Camera.main.transform.position.z));
        if (icon.activeSelf.Equals(true))
        {
            icon.transform.position = mousePosition;
        }
        Debug.DrawRay((Vector3)mousePosition + Vector3.back * 25, Vector3.forward * 50, Color.red);
    }

    #region INPUT CALLBACKS
    public void OnInventory()
    {
        ToggleInventory();
    }

    public void OnPickup()
    {
        if (interactableItems.Length > 0)
        {
            PickUp();
            StartCoroutine(StatusEffects.SlowTarget(gameObject, 0.60f, 0.5f));
            #region Animation
            animator.SetTrigger("Pickup");
            #endregion
        }
    }

    public void OnLeftClick(InputValue value) // user clicks the left mouse button
    {
        float clickValue = value.Get<float>();

        if (clickValue == 1 && isOpen) // button down
        {
            Slot slot = InventorySystem.FindSlot(mousePosition);
            if(slot == null && itemHolding != null) // drop item if placed outside the inventory space
            {
                InventorySystem.DropItem(itemHolding);
                inventoryCount--;
                icon.SetActive(false);
                itemHolding = null;
            }
            else if (slot == null) // return if clicked on nothing
            {
                return;
            }
            else if (slot.item == null && itemHolding != null) //place item if slot available
            {
                InventorySystem.TransferItem(itemHolding, slot);
                if(slot.item != null)
                {
                    icon.SetActive(false);
                    itemHolding = null;
                }                              
            }
            else if (slot.item != null && itemHolding != null) //drop item if slot is not available and holding item
            {
                InventorySystem.DropItem(itemHolding);
                inventoryCount--;
                icon.SetActive(false);
                itemHolding = null;
            }
            else if (slot.item != null && itemHolding == null) //move item if slot has an item and not holding an item
            {
                itemHolding = slot.item;
                InventorySystem.EmptySlot(slot);
                MoveItem(itemHolding);
            }
        }
    }

    #endregion

    public void MoveItem(Item item)
    {             
        icon.transform.position = mousePosition;
        icon.SetActive(true);
        icon.GetComponent<Image>().sprite = item.sprite;
    }

    public void ToggleInventory()
    {
        if (!isOpen)
        {
            PlayerSettings.DisableControl();
            isOpen = true;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            inventoryContainer.gameObject.SetActive(true);
        }
        else
        {
            PlayerSettings.EnableControl();
            isOpen = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            inventoryContainer.gameObject.SetActive(false);
        }
    }

    public void PickUp()
    { // pick up closest item
        if (inventoryCount != 20)
        {
            Item item = GetClosestItem(transform.position, interactableItems).GetComponent<Stats>().source;
            GameObject itemObj = GetClosestItem(transform.position, interactableItems);
            for(int i = 0; i < inventorySlotsContainer.transform.childCount; i++)
            {
                Slot slot = inventorySlotsContainer.transform.GetChild(i).GetComponent<Slot>();
                if (slot.item == null)
                {
                    InventorySystem.TransferItem(item, slot);              
                    Destroy(itemObj);                 
                    inventoryCount++;
                    i = 9999;
                }
            }
        }
        else
        {
            Debug.Log("Inventory Full");
        }
    }

    public void ScanInteractArea(SphereCollider playerCollider)
    { // check for interactables  
        interactableItems = Physics.OverlapSphere(transform.position + new Vector3(0, playerZone.center.y, 0), playerCollider.radius, contactFilter);


        if (interactableItems.Length > 0)
        {
            GameObject item = GetClosestItem(transform.position, interactableItems);
            CanvasDisplay.DisplayInteractText(item.transform.position, "E");
        }
        else
        {
            CanvasDisplay.HideText();
        }
    }

    public GameObject GetClosestItem(Vector3 playerPos, Collider[] interactableItems)
    { // returns back the closest item
        GameObject closestItem = interactableItems[0].gameObject;
        float minDistance = Vector3.Distance(playerPos, interactableItems[0].gameObject.transform.position);
        for (int i = 0; i < interactableItems.Length; i++)
        {
            if (minDistance > Vector3.Distance(playerPos, interactableItems[i].gameObject.transform.position))
            {
                minDistance = Vector3.Distance(playerPos, interactableItems[i].gameObject.transform.position);
                closestItem = interactableItems[i].gameObject;
            }
        }
        return closestItem;
    }
}
