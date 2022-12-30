using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMaster : MonoBehaviour
{
    public static CameraMaster instance;

    public enum CMCams { mainCam, aimCam, inventoryCam }

    [Header("Cameras")]
    public GameObject aimCam;
    public GameObject inventoryCam;
    public GameObject mainCam;

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
    }

    public void SwitchToCamera(CMCams cam)
    {
        aimCam.SetActive(false);
        inventoryCam.SetActive(false);
        mainCam.SetActive(false);

        switch (cam) 
        {
            case CMCams.mainCam:
                mainCam.SetActive(true);
                break;

            case CMCams.aimCam:
                aimCam.SetActive(true);
                break;

            case CMCams.inventoryCam:
                inventoryCam.SetActive(true);
                break;

            default:
                mainCam.SetActive(true);
                break;
        }       
    }
}
