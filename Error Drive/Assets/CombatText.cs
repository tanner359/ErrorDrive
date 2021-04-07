using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombatText : MonoBehaviour
{
    public TextMeshPro meshPro;

    Color color;
    int damage;
    float duration;
    Vector3 location;

    public CombatText(Color _color, int _damage, float _duration, Vector3 _location)
    {
        color = _color;
        damage = _damage;
        duration = _duration;
        location = _location;
    }

    private void Awake()
    {
        transform.position = location + new Vector3(Random.Range(-2f,2f),0,0);     
        meshPro.color = color;
        meshPro.text = damage.ToString();
        Destroy(gameObject, duration);
    }
}
