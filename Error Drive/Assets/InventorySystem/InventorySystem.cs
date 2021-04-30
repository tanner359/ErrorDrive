using UnityEngine;

public static class InventorySystem
{
    public static LayerMask slotMask = LayerMask.GetMask("Inventory Slot");
    public static void TransferItem(Item item, Slot slot)
    {
        if (slot.TryGetComponent(out Equip equip))
        {
            if (slot.tag == item.equipSlot.ToString())
            {
                for (int i = 0; i < equip.bodyParts.Count; i++)
                {
                    equip.bodyParts[i].GetComponent<MeshFilter>().mesh = item.mesh;
                    equip.bodyParts[i].GetComponent<MeshRenderer>().material = item.material;
                    equip.IgnorePartsSetActive(false);
                    Combat.player.AddStats(item);
                }
                if (item.itemClass == Item.ItemClass.Melee)
                {
                    MeshCollider meshCol = equip.bodyParts[0].GetComponent<MeshCollider>();
                    meshCol.enabled = true;
                    meshCol.sharedMesh = item.mesh;
                }
                if (item.itemClass == Item.ItemClass.Ranged)
                {
                    Player_Controller controller = Combat.player.GetComponent<Player_Controller>();
                    if (item.equipSlot == Item.EquipType.Main_Hand)
                    {
                        controller.aimRight = true;
                        controller.RefreshIK();
                    }
                    else if (item.equipSlot == Item.EquipType.Off_Hand)
                    {
                        controller.aimLeft = true;
                        controller.RefreshIK();
                    }
                }
            }
            else
            {
                Debug.Log("Wrong Equip Slot");
                slot.item = null;
                slot.image.enabled = false;
                return;
            }
        }
        slot.item = item;
        slot.image.enabled = true;
        slot.image.sprite = item.sprite;
        slot.slotImage.color = ItemSystem.GetRarityColor(item.rarity);
        //slot.label.text = item.itemName; [REMOVED] => Sets label name
        //slot.label.color = ItemSystem.GetRarityColor(item.rarity); [REMOVED] => Sets label color
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
        Transform tran = Combat.player.transform;
        ItemSystem.Spawn(item, tran.position + tran.forward * 3 + Vector3.up * 3);
    }

    public static void EmptySlot(Slot slot)
    {

        slot.image.sprite = null;
        //slot.label.text = ""; [REMOVED] => Clears label text
        slot.image.enabled = false;
        slot.slotImage.color = Color.white;

        if (slot.TryGetComponent(out Equip equip))
        {
            slot.label.text = slot.tag.ToString().Replace('_', ' ');
            slot.label.color = Color.white;
            slot.slotImage.color = Color.white;
            for (int i = 0; i < equip.bodyParts.Count; i++)
            {
                equip.bodyParts[i].GetComponent<MeshFilter>().mesh = equip.GetOriginalMeshes(i);
                equip.bodyParts[i].GetComponent<MeshRenderer>().material = equip.GetOriginalMaterials(i);
                equip.IgnorePartsSetActive(true);
                Combat.player.RemoveStats(slot.item);
            }
            if (slot.item.itemClass == Item.ItemClass.Melee)
            {
                MeshCollider meshCol = equip.bodyParts[0].GetComponent<MeshCollider>();
                meshCol.enabled = false;
                meshCol.sharedMesh = null;
            }
            if (slot.item.itemClass == Item.ItemClass.Ranged)
            {
                Player_Controller controller = Combat.player.GetComponent<Player_Controller>();
                if (slot.item.equipSlot == Item.EquipType.Main_Hand)
                {
                    controller.aimRight = false;
                    controller.RefreshIK();
                }
                else if (slot.item.equipSlot == Item.EquipType.Off_Hand)
                {
                    controller.aimLeft = false;
                    controller.RefreshIK();
                }
            }
        }
        slot.item = null;
    }

    public static Slot GetEquipSlot(Equip.EquipSlot equipTag)
    {
        Transform equipSlots = CanvasDisplay.instance.equipSlotsContent;
        for (int i = 0; i < equipSlots.childCount; i++)
        {
            if (equipSlots.GetChild(i).CompareTag(equipTag.ToString()))
            {
                return equipSlots.GetChild(i).GetComponent<Slot>();
            }
        }
        Debug.LogError("Slot not Found");
        return null;
    }

    public static Item[] GetItemsEquipped()
    {
        Item[] items = new Item[5];

        for(int i = 0; i < 5; i++)
        {
            items[i] = GetEquipSlot(Equip.EquipSlot.Main_Hand + i).item;
        }
        return items;
    }
}
