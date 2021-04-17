using UnityEngine;

public static class InventorySystem
{
    public static LayerMask slotMask = LayerMask.GetMask("Inventory Slot");
    public static void TransferItem(Item item, Slot slot)
    {
        slot.item = item;
        slot.image.enabled = true;
        slot.image.sprite = item.sprite;
        slot.label.text = item.itemName;
        slot.label.color = ItemSystem.GetRarityColor(item.rarity);

        if(slot.TryGetComponent(out Equip equip))
        {
            for(int i = 0; i < equip.bodyParts.Count; i++)
            {
                equip.bodyParts[i].GetComponent<MeshFilter>().mesh = item.mesh;
                equip.bodyParts[i].GetComponent<MeshRenderer>().material = item.material;
                equip.IgnorePartsSetActive(false);
                Combat.player.AddStats(item);
            }
        }
    }

    public static Slot FindSlot(Vector3 position)
    {
        Physics.Raycast(position + Vector3.back * 25, Vector3.forward * 50, out RaycastHit hit, slotMask);
        if (hit.collider != null)
        {
            return hit.collider.GetComponent<Slot>();
        }
        else
        {
            return null;
        }

    }

    public static void DropItem(Item item)
    {
        Transform tran = GameObject.FindGameObjectWithTag("Player").transform;
        ItemSystem.Spawn(item, tran.position + tran.forward * 3 + Vector3.up * 3);
    }

    public static void EmptySlot(Slot slot)
    {
        
        slot.image.sprite = null;
        slot.label.text = "";
        slot.image.enabled = false;

        if (slot.TryGetComponent(out Equip equip))
        {
            slot.label.text = slot.tag.ToString().Replace('_', ' ');
            slot.label.color = Color.white;
            for (int i = 0; i < equip.bodyParts.Count; i++)
            {
                equip.bodyParts[i].GetComponent<MeshFilter>().mesh = equip.GetOriginalMeshes(i);
                equip.bodyParts[i].GetComponent<MeshRenderer>().material = equip.GetOriginalMaterials(i);
                equip.IgnorePartsSetActive(true);            
                Combat.player.RemoveStats(slot.item);
            }
        }
        slot.item = null;
    }
}
