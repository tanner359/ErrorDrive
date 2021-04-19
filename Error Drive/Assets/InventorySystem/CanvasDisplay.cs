﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasDisplay : MonoBehaviour
{
    public static CanvasDisplay instance;

    private static GameObject textBox;
    public GameObject interactText_Prefab;
    public GameObject worldCanvas;
    public GameObject reticle;
    public GameObject tooltip;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        textBox = Instantiate(interactText_Prefab, worldCanvas.transform);
    }

    public static void DisplayInteractText(Vector3 textPos, string text)
    {
        textBox.SetActive(true);
        textBox.GetComponent<TMPro.TextMeshPro>().text = text;
        textBox.transform.position = textPos + Vector3.up * 3;
    }

    public void TooltipActive(bool state)
    {
        tooltip.SetActive(state);
    }

    public static void HideText()
    {
        textBox.SetActive(false);
    }

    public void CloseWindow(GameObject window)
    {
        PlayerSettings.EnableControl();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        window.SetActive(false);
    }
}
