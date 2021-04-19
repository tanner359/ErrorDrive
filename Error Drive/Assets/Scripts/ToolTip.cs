using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ToolTip : MonoBehaviour
{
    public static ToolTip instance;

    public static string equipTypeText, labelText, descriptionText, powerText,
            critText, penText, healthText, defenseText, speedText, knockbackText, levelReqText;

    static List<string> itemDetails = new List<string>();
    public List<Text> Texts = new List<Text>();

    public static Color rarityColor;

    public Item inspectedItem;

    private void Awake()
    {
        if(instance!= null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    public void UpdateToolTip(Item item)
    {
        inspectedItem = item;
        itemDetails.Clear();
        equipTypeText = item.equipSlot.ToString();
        itemDetails.Add(equipTypeText);
        labelText = item.itemName;
        itemDetails.Add(labelText);
        descriptionText = '"' + item.description + '"';
        itemDetails.Add(descriptionText);
        powerText = "+" + item.power.ToString() + " Power";
        itemDetails.Add(powerText);
        critText = "+" + item.crit.ToString() + " Critical Strike";
        itemDetails.Add(critText);
        penText = "+" + item.pen.ToString() + " Penetration";
        itemDetails.Add(penText);
        healthText = "+" + item.health.ToString() + " Health";
        itemDetails.Add(healthText);
        defenseText = "+" + item.defense.ToString() + " Defense";
        itemDetails.Add(defenseText);
        speedText = "+" + item.speed.ToString() + " Speed";
        itemDetails.Add(speedText);
        knockbackText = "+" + item.knockback.ToString() + " Knockback";
        itemDetails.Add(knockbackText);
        levelReqText = "Level Requirement: " + item.levelRequirement;
        itemDetails.Add(levelReqText);
        rarityColor = ItemSystem.GetRarityColor(item.rarity);

        for (int i = 0; i < itemDetails.Count; i++)
        {
            if (itemDetails[i][1] == '0')
            {
                Texts[i].text = "";
                Texts[i].transform.parent.gameObject.SetActive(false);
            }
            else
            {
                Texts[i].transform.parent.gameObject.SetActive(true);
                Texts[i].text = itemDetails[i];
            }
        }
        Texts[1].color = rarityColor;
    }
}
