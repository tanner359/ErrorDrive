using UnityEngine;
using UnityEngine.UI;
public class Slot : MonoBehaviour
{
    [Header("Slotted Item")]
    public Item item;

    [Header("References")]
    public Image image;
    public RawImage slotImage;
    public Text label;
}
