using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticle : MonoBehaviour
{

    [Range(1f, 20f)]
    public float range;
    public LayerMask targetMask;

    private void Update()
    {       
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward * range);
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * range, Color.red);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 20f, targetMask, QueryTriggerInteraction.Ignore);
        if (hit.collider != null)
        {
            Debug.Log(hit.collider);
            transform.position = hit.point - transform.forward;
        }
        else
        {
            transform.position = Camera.main.transform.position + (Camera.main.transform.forward * range);
        }
    }
}
