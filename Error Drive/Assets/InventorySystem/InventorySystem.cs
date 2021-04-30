using UnityEngine;
using System.Collections.Generic;
public static class InventorySystem
{
    public static LayerMask slotMask = LayerMask.GetMask("Inventory Slot");
    public static void TransferItem(Item item, Slot slot)
    {
        if (slot.gameObject.layer == 15)
        {          
            if (slot.tag == item.equipSlot.ToString())
            {
                EquipmentLink link = Combat.player.GetComponent<EquipmentLink>();
                link.bodyLinks.TryGetValue(item.equipSlot, out List<GameObject> links);
                for (int i = 0; i < item.meshes.Count; i++)
                {
                    links[(links.Count - 1) - i].GetComponent<MeshFilter>().mesh = item.meshes[i];
                    links[(links.Count - 1) - i].GetComponent<MeshRenderer>().material = item.material;
                    link.IgnorePartsSetActive(item.equipSlot, false);
                    Combat.player.AddStats(item);
                }
                if (item.itemClass == Item.ItemClass.Melee)
                {
                    MeshCollider meshCol = links[0].GetComponent<MeshCollider>();
                    meshCol.enabled = true;
                    meshCol.sharedMesh = item.meshes[0];
                }
                if (item.itemClass == Item.ItemClass.Ranged)
                {
                    Player_Controller controller = Combat.player.GetComponent<Player_Controller>();
                    if (item.equipSlot == Item.EquipSlot.Main_Hand)
                    {
                        controller.aimRight = true;
                        controller.RefreshIK();
                    }
                    else if (item.equipSlot == Item.EquipSlot.Off_Hand)
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
        slot.image.enabled = false;
        slot.slotImage.color = Color.white;

        if (slot.gameObject.layer == 15)
        {
            slot.label.text = slot.tag.ToString().Replace('_', ' ');
            slot.label.color = Color.white;
            slot.slotImage.color = Color.white;

            EquipmentLink link = Combat.player.GetComponent<EquipmentLink>();
            link.bodyLinks.TryGetValue(slot.item.equipSlot, out List<GameObject> links);

            for (int i = 0; i < slot.item.meshes.Count; i++)
            {
                links[(links.Count - 1) - i].GetComponent<MeshFilter>().mesh = link.GetOriginalMeshes(slot.item.equipSlot, i);
                links[(links.Count - 1) - i].GetComponent<MeshRenderer>().material = link.GetOriginalMaterials(slot.item.equipSlot, i);
                link.IgnorePartsSetActive(slot.item.equipSlot, true);
                Combat.player.RemoveStats(slot.item);
            }
            if (slot.item.itemClass == Item.ItemClass.Melee)
            {
                MeshCollider meshCol = links[0].GetComponent<MeshCollider>();
                meshCol.enabled = false;
                meshCol.sharedMesh = null;
            }
            if (slot.item.itemClass == Item.ItemClass.Ranged)
            {
                Player_Controller controller = Combat.player.GetComponent<Player_Controller>();
                if (slot.item.equipSlot == Item.EquipSlot.Main_Hand)
                {
                    controller.aimRight = false;
                    controller.RefreshIK();
                }
                else if (slot.item.equipSlot == Item.EquipSlot.Off_Hand)
                {
                    controller.aimLeft = false;
                    controller.RefreshIK();
                }
            }
        }
        slot.item = null;
    }

    public static Slot GetEquipSlot(Item.EquipSlot equipTag)
    {
        Debug.Log(CanvasDisplay.instance.equipSlotsContent);
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
        Item[] items = new Item[6];

        for(int i = 0; i < 5; i++)
        {
            items[i] = GetEquipSlot(Item.EquipSlot.Main_Hand + i).item;
        }
        return items;
    }
}
