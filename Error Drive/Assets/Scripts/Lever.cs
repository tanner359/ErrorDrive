using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lever : MonoBehaviour
{
    public GameObject door;

    public bool open = false;

    public GameObject[] lightSources;
    public Material greenLight;
    public Light[] lights;


    void OnTriggerEnter(Collider player)
    {
        if (player.tag == "Player" && !open)
        {
            door.transform.position += Vector3.up * 10.0f;

            transform.eulerAngles = new Vector3(
            transform.eulerAngles.x,
            transform.eulerAngles.y,
            transform.eulerAngles.z - 100);




            foreach (GameObject lightSource in lightSources)
            {
                lightSource.GetComponent<MeshRenderer>().material = greenLight;
            }

            foreach(Light light in lights)
            {
                light.color = Color.green;
            }


            open = true;
        }
    }
}
