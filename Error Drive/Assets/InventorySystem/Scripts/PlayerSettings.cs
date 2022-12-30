using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerSettings
{
    static Controller controller = Object.FindObjectOfType<Controller>();

    public static void DisableControl()
    {
        controller.isControlling = false;
    }
    public static void EnableControl()
    {
        controller.isControlling = true;
    }
}
