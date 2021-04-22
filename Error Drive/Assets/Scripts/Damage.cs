using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public Stats stats;
    public Player_Controller controller;  

    private void OnTriggerEnter(Collider other) // if we hit something, apply damage
    {
        if (controller.isAttacking && other.CompareTag("Hostile"))
        {
            Debug.Log("Hostile Hit");
            Combat.DamageTarget(other.GetComponent<Stats>(), stats);
            controller.isAttacking = false;
        }
    }

}
