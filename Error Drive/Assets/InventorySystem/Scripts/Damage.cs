using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public Stats stats;
    public Player_Controller controller;
    public Item.EquipSlot equipSlot;

    private void OnTriggerEnter(Collider other) // if we hit something, apply damage
    {
        if (controller.isAttacking && other.CompareTag("Hostile"))
        {
            Item weapon = InventorySystem.GetEquipSlot(equipSlot).item;
            Combat.DamageTarget(weapon, other.GetComponent<Stats>(), stats);
            other.GetComponent<Enemy>().DisableAgent();
            other.GetComponent<Rigidbody>().AddForce(((other.transform.position - transform.position) * weapon.knockback) + Vector3.up * 2, ForceMode.Impulse);
            controller.isAttacking = false;
        }
    }
}
