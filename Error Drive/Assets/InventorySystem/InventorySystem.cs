using UnityEngine;

public static class InventorySystem
{
    public static void TransferItem(Slot origin, Slot destination)
    {
        destination.item = origin.item;
        origin.item = null;
    }

    public static Slot FindSlot(Vector3 position)
    {
        Slot slot;
        Physics.Raycast(position + Vector3.back * 25, Vector3.forward * 50, out RaycastHit hit);
        slot = hit.collider.GetComponent<Slot>();
        return slot;        
    }

    public static void DropItem(Item item)
    {
        Transform tran = GameObject.FindGameObjectWithTag("Player").transform;
        ItemSystem.Spawn(item, tran.position + tran.forward * 3 + Vector3.up * 3);
    }
}
