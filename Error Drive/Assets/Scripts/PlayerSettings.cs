using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerSettings
{
    static Player_Controller controller = Object.FindObjectOfType<Player_Controller>();

    public static void DisableControl()
    {
        controller.isControlling = false;
    }
    public static void EnableControl()
    {
        controller.isControlling = true;
    }
}
