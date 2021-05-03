using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public int secondsBetween;
    public GameObject objectToSpawn;


    void Start()
    {
        StartCoroutine("SpawnObject", secondsBetween);
    }

    private IEnumerator SpawnObject(int seconds)
    {
        while (true)
        {
            yield return new WaitForSeconds(seconds);

            Instantiate(objectToSpawn, transform.position, transform.rotation);
        }
    }
}
