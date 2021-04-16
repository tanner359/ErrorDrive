using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public Item item;

    public static void TransferItem(Slot origin, Slot destination)
    {
        destination.item = origin.item;
    }
}
