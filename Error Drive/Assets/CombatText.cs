using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombatText : MonoBehaviour
{
    public TextMeshPro meshPro;

    static Color color;
    static int damage;
    static float duration;
    static Vector3 location;

    public static void CombatTextInfo(Color _color, int _damage, float _duration, Vector3 _location)
    {
        color = _color;
        damage = _damage;
        duration = _duration;
        location = _location;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up, 0.01f);
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 0.01f);
    }

    private void Awake()
    {
        transform.position = location + new Vector3(Random.Range(-2f,2f),0,0);     
        meshPro.color = color;
        meshPro.text = damage.ToString();
        Destroy(gameObject, duration);
    }
}
