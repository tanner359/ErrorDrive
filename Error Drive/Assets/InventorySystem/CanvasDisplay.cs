using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasDisplay : MonoBehaviour
{
    public Vector3 pickupTextOffset = new Vector3(0, 0, 0);
    private GameObject textBox;
    public int damageTextLimit;
    public GameObject damageTextPrefab;
    public TMP_FontAsset fontAsset;
    public GameObject worldCanvas;

    public List<GameObject> damageTextBoxes = new List<GameObject>();

      
    private void Update()
    {
        for (int i = 0; i < damageTextBoxes.Count; i++)
        {           
            if (damageTextBoxes[i] != null)
            {
                damageTextBoxes[i].transform.position += new Vector3(0, 2f * Time.deltaTime, 0);
            }
            else
            {
                damageTextBoxes.RemoveAt(i);
            }          
        }
    }
    private void Awake()
    {
        textBox = new GameObject("DamageText");
        textBox.transform.SetParent(worldCanvas.transform);
        textBox.AddComponent<TMPro.TextMeshPro>();
        textBox.GetComponent<TMPro.TextMeshPro>().font = fontAsset;
        textBox.GetComponent<TMPro.TextMeshPro>().fontSize = 8;
        textBox.GetComponent<TMPro.TextMeshPro>().outlineWidth = 0.32f;
        textBox.GetComponent<TMPro.TextMeshPro>().alignment = TextAlignmentOptions.Center;
        textBox.GetComponent<RectTransform>().sizeDelta = new Vector2(2, 1);
    }
    public void displayInteractText(Transform textPos, string text)
    {
        textBox.GetComponent<TMPro.TextMeshPro>().text = text;
        textBox.transform.position = textPos.position + pickupTextOffset;
    }
    public void HideText()
    {
        textBox.GetComponent<TMPro.TextMeshPro>().text = "";
    }
    public void displayDamageText(Color color, int damage, float duration, Transform location)
    {
        GameObject damageTextBox = GameObject.Instantiate(damageTextPrefab);
        damageTextBox.transform.position = location.position + new Vector3(Random.Range(-2f,2f),0,0);     
        damageTextBox.GetComponent<TMPro.TextMeshPro>().color = color;
        damageTextBox.GetComponent<TMPro.TextMeshPro>().text = damage.ToString();
        damageTextBoxes.Add(damageTextBox);       
        Destroy(damageTextBox, duration);    
        
    }

    public void CloseWindow(GameObject window)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Controller>().SetControl(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        window.SetActive(false);
    }
    

}
