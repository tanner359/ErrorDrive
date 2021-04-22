using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reticle : MonoBehaviour
{
    public static Reticle instance;
    public Image image;
    public GameObject mainCam;

    [Range(1f, 100f)]
    public float range;
    public LayerMask targetMask;


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
    private void Update()
    {
        Ray ray;

        if (CanvasDisplay.instance.inventory.gameObject.activeInHierarchy)
        {
            ray = new Ray(mainCam.transform.position, Combat.player.transform.forward * range);
        }
        else
        {
            ray = new Ray(mainCam.transform.position, mainCam.transform.forward * range);
        }

        RaycastHit hit;
        Physics.Raycast(ray, out hit, range, targetMask, QueryTriggerInteraction.Ignore);
        if (hit.collider != null)
        {
            float distance = Vector3.Distance(hit.point, mainCam.transform.position);
            if (distance < range && distance > 6f)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, (distance / range) * 1.0f); 
            }
            else if(distance < 6f)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
            }
            transform.position = hit.point - transform.forward;
        }
        else
        {
            transform.position = Camera.main.transform.position + (Camera.main.transform.forward * range);
        }
    }
}
