using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFunctions : MonoBehaviour
{
    public Player_Controller controller;

    public void EnableAttack()
    {
        controller.isAttacking = true;
    }

    public void DisableAttack()
    {
        controller.isAttacking = false;
    }
}
