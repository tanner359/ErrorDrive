using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBehaviour : MonoBehaviour
{

    public float itemZ;

    private void Start()
    {
        itemZ = gameObject.transform.parent.transform.position.z;
    }


    void FixedUpdate()
    {
        gameObject.transform.eulerAngles = new Vector3(Mathf.Abs(itemZ) + (-90), -90, 0);
    }           
}
