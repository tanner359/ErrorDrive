using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public Stats stats;
    public Player_Controller controller;
    public Equip.Tags handSlot;

    private void OnTriggerEnter(Collider other) // if we hit something, apply damage
    {
        if (controller.isAttacking && other.CompareTag("Hostile"))
        {
            Item weapon = InventorySystem.GetEquipSlot(handSlot).item;
            Combat.DamageTarget(weapon, other.GetComponent<Stats>(), stats);
            other.GetComponent<Enemy>().DisableAgent();
            other.GetComponent<Rigidbody>().AddForce(((other.transform.position - transform.position) * stats.knockback) + Vector3.up * 2, ForceMode.Impulse);
            controller.isAttacking = false;
        }
    }

}
