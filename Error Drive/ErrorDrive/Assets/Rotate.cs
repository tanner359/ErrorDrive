using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{


    public Transform target;
    public Component test;
    public Rigidbody test2;


    void Update()
    {
        // Rotate the camera every frame so it keeps looking at the target
        transform.LookAt(target);
        if (Input.GetMouseButtonDown(0))
        {

            Destroy(test);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            test2.useGravity = true;
            test2.AddForce(transform.up * 150.0f);
            Destroy(this);
        }

    }







}
