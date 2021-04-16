using UnityEngine;

public static class InventorySystem
{
    public static void TransferItem(Item item, Slot destination)
    {
        destination.item = item;
    }

    public static Slot FindSlot(Vector3 position)
    {
        Physics.Raycast(position + Vector3.back * 25, Vector3.forward * 50, out RaycastHit hit);
        if (hit.collider.GetComponent<Slot>() == null)
        {
            return null;
        }
        else
        {
            return hit.collider.GetComponent<Slot>();
        }
    }

    public static void DropItem(Item item)
    {
        Transform tran = GameObject.FindGameObjectWithTag("Player").transform;
        ItemSystem.Spawn(item, tran.position + tran.forward * 3 + Vector3.up * 3);
    }

    public static void EmptySlot(Slot slot)
    {
        slot.item = null;       
        slot.image.sprite = null;
        slot.label.text = "";
        slot.image.enabled = false;
    }
}
