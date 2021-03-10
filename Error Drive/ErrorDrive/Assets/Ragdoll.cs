using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    Rigidbody[] rbArray;
    InverseKinematics[] IKArray;
    CharacterJoint[] cJointArray;

    public bool ragdollOn = false;

    // Start is called before the first frame update
    void Start()
    {
        rbArray = gameObject.GetComponentsInChildren<Rigidbody>();
        IKArray = gameObject.GetComponentsInChildren<InverseKinematics>();



        foreach (Rigidbody rb in rbArray)
        {
            rb.isKinematic = true;
        }

        foreach (InverseKinematics IK in IKArray)
        {
            IK.enabled = true;
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {

            if (!ragdollOn)
            {
                Debug.Log("Hi");
                EnableRagdoll();
                ragdollOn = true;
            }
            else
            {
                Debug.Log("Ho");
                DisableRagdoll();
                ragdollOn = false;
            }

        }

        if (Input.GetKeyDown("down"))
        {
            EnableRagdoll();
            LimbExplosion();
        }


        if (Input.GetKeyDown("up"))
        {
            EnableRagdoll();
            DeathExplosion();
            Debug.Log("Howdy!");
            /*if (!ragdollOn)
            {
                Debug.Log("Hi");
                EnableRagdoll();
                
                ragdollOn = true;
            }
            else
            {
                Debug.Log("Ho");
                DisableRagdoll();
                ragdollOn = false;
            }*/

        }
    }


    void EnableRagdoll()
    {
        rbArray = gameObject.GetComponentsInChildren<Rigidbody>();
        IKArray = gameObject.GetComponentsInChildren<InverseKinematics>();

        foreach (Rigidbody rb in rbArray)
        {
            rb.isKinematic = false;
        }

        foreach (InverseKinematics IK in IKArray)
        {
            IK.enabled = false;
        }
    }


    void DisableRagdoll()
    {
        rbArray = gameObject.GetComponentsInChildren<Rigidbody>();
        IKArray = gameObject.GetComponentsInChildren<InverseKinematics>();

        foreach (Rigidbody rb in rbArray)
        {
            rb.isKinematic = true;
        }

        foreach (InverseKinematics IK in IKArray)
        {
            IK.enabled = true;
        }
    }


    void DeathExplosion()
    {
        cJointArray = gameObject.GetComponentsInChildren<CharacterJoint>();

        foreach (CharacterJoint cj in cJointArray)
        {
            Destroy(cj);
        }
    }

    void LimbExplosion()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject.GetComponent<CharacterJoint>());
        }
    }
}
