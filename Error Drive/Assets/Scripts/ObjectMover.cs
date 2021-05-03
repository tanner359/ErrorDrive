using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public int secondsOfMovement;
    public float speed;

    void Start()
    {
        StartCoroutine("ReverseDirection", secondsOfMovement);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    private IEnumerator ReverseDirection(int seconds)
    {
        while (true)
        {
            yield return new WaitForSeconds(seconds);

            speed *= -1;
        }

    }
}
