using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public int secondsOfMovement;
    public float speed;
    public bool turnAround = true;
    public bool destroy;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;

        if (turnAround)
        {
            StartCoroutine("ReverseDirection", secondsOfMovement);
        }
        else if(!destroy)
        {
            StartCoroutine("TeleportBack", secondsOfMovement);
        }
        else
        {
            StartCoroutine("Kill", secondsOfMovement);
        }
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

    private IEnumerator TeleportBack(int seconds)
    {
        while (true)
        {
            yield return new WaitForSeconds(seconds);

            transform.position = startPos;
        }

    }

    private IEnumerator Kill(int seconds)
    {
        while (true)
        {
            yield return new WaitForSeconds(seconds);

            Destroy(gameObject);
        }

    }
}
