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

    public static void CombatTextInfo(Color _color, int _damage, float _duration)
    {
        color = _color;
        damage = _damage;
        duration = _duration;
    }

    private void Update()
    {
        //transform.LookAt(Camera.main.transform, Vector3.forward);
        transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up, 0.01f);
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 0.01f);
    }

    private void Awake()
    {
        transform.position += new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        meshPro.color = color;
        meshPro.text = damage.ToString();
        Destroy(gameObject, duration);
    }
}
