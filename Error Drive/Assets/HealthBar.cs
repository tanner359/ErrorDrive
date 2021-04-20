using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Stats target;

    public Image healthbar;

    public Image flash;

    private float maxHealth;

    private float currentHealth;

    public Text nameTag;

    private void Start()
    {
        nameTag.text = target.gameObject.name;
        maxHealth = target.health;
        currentHealth = target.health;
        transform.SetParent(GameObject.Find("Screen_Canvas").transform);
        transform.localScale = new Vector3(1, 1, 1);
    }

    private void Update()
    {
        transform.position = target.transform.position + Vector3.up * 4;

        if (target.health != currentHealth)
        {
            currentHealth = target.health;
            healthbar.fillAmount = currentHealth / maxHealth;

            StartCoroutine(DamageFlash());
        }
    }

    public IEnumerator DamageFlash()
    {     
        flash.enabled = true;
        float posX = healthbar.fillAmount * healthbar.rectTransform.rect.width;
        flash.transform.localPosition = new Vector3(posX - 7f, 0, 0);
        yield return new WaitForSeconds(0.1f);
        flash.enabled = false;
    }

}
