using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lootContainer : MonoBehaviour
{
    public GameObject containerCover;


    public void OpenContainer()
    {
        containerCover.AddComponent<Rigidbody>().AddForce(containerCover.transform.right * 10, ForceMode.Impulse);
        ItemSystem.CalculateDrops(containerCover.transform.position);
        gameObject.layer = 7;
    }
}
