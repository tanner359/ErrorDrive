using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasDisplay : MonoBehaviour
{
    public Vector3 pickupTextOffset = new Vector3(0, 0, 0);
    private GameObject textBox;
    public GameObject combatText_Prefab;
    public GameObject interactText_Prefab;
    public GameObject worldCanvas;
         
    private void Awake()
    {
        textBox = Instantiate(interactText_Prefab, worldCanvas.transform);
    }

    public void DisplayInteractText(Vector3 textPos, string text)
    {
        textBox.SetActive(true);
        textBox.GetComponent<TMPro.TextMeshPro>().text = text;
        textBox.transform.position = textPos + pickupTextOffset;
    }

    public void HideText()
    {
        textBox.SetActive(false);
    }

    public void SpawnCombatText(CombatText combatText)
    {
        Instantiate(combatText_Prefab);                 
    }

    public void CloseWindow(GameObject window)
    {
        PlayerSettings.EnableControl();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        window.SetActive(false);
    }
}
