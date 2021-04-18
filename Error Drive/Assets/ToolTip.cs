using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.InputSystem;

public class ToolTip : MonoBehaviour
{
    Player_Inputs playerInputs;

    public static string equipTypeText, labelText, descriptionText, powerText,
            critText, penText, healthText, defenseText, speedText, knockbackText, levelReqText;

    static List<string> itemDetails = new List<string>();
    public List<Text> Texts = new List<Text>();

    public static Color rarityColor;

    public static Item inspectedItem;
    

    public static void UpdateToolTip(Item item)
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
    }

    private void OnEnable()
    {
        if (playerInputs == null)
        {
            playerInputs = new Player_Inputs();
        }
        playerInputs.Player.Enable();

        for (int i = 0; i < itemDetails.Count; i++)
        {
            if (itemDetails[i].Count(f => (f == '0')) == 0)
            {
                Texts[i].text = "";
                Texts[i].transform.parent.gameObject.SetActive(false);
            }
            else
            {
                Texts[i].transform.parent.gameObject.SetActive(true);
                Texts[i].text = equipTypeText;
            }
        }
        Texts[1].color = rarityColor;
    }

    private void Update()
    {
        Vector2 mousePosition = new Vector3(playerInputs.Player.MousePosition.ReadValue<Vector2>().x, playerInputs.Player.MousePosition.ReadValue<Vector2>().y, Mathf.Abs(Camera.main.transform.position.z));
        transform.position = mousePosition;
    }
}
