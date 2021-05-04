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
        if (!target.gameObject.CompareTag("Player"))
        {
            nameTag.text = target.gameObject.name;
            transform.SetParent(GameObject.Find("Screen_Canvas").transform);
            transform.localScale = new Vector3(1, 1, 1);
        }       
        maxHealth = target.health;
        currentHealth = target.health;     
    }

    private void Update()
    {
        if (!target.gameObject.CompareTag("Player"))
        {
            transform.position = target.transform.position + Vector3.up * 4;
        }

        if (target.health != currentHealth)
        {
            recoveryTime = 15f;
            currentHealth = target.health;
            healthbar.fillAmount = currentHealth / maxHealth;
            if (!target.gameObject.CompareTag("Player"))
            {
                if (!flash.enabled)
                {
                    StartCoroutine(DamageFlash());
                }
                if (target.health <= 0)
                {
                    Destroy(target.gameObject);
                    Destroy(gameObject);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        Timer();
    }

    public float recoveryTime;
    public void Timer()
    {
        if(recoveryTime >= 0)
        {
            recoveryTime -= Time.deltaTime;
        }
        else if(recoveryTime <= 0 && currentHealth < maxHealth)
        {
            target.health = (int)Mathf.Lerp(target.health, target.health + (target.health * 0.02f), 0.05f);
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
