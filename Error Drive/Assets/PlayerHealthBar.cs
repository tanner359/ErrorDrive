using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthBar : MonoBehaviour
{
    public Stats target;

    public Image healthbar;

    public TMP_Text label;

    private int currentHealth;

    bool isHealing = true;

    private void Start()
    {
        currentHealth = target.maxHealth;
    }

    private void Update()
    {
        if(target.maxHealth < target.health)
        {          
            target.health = target.maxHealth;
            currentHealth = target.health;
            healthbar.fillAmount = currentHealth / (float)target.maxHealth;
        }
        if(target.health < currentHealth)
        {
            StopAllCoroutines();
            currentHealth = target.health;
            healthbar.fillAmount = currentHealth / (float)target.maxHealth;
            StartCoroutine(RecoveryDelay(8f));
        }
        else if(target.health > currentHealth)
        {
            currentHealth = target.health;
            healthbar.fillAmount = currentHealth / (float)target.maxHealth;
        }
        else if(target.health < target.maxHealth && isHealing)
        {
            isHealing = false;
            currentHealth = target.health;
            healthbar.fillAmount = currentHealth / (float)target.maxHealth;
            StartCoroutine(Heal());
        }
        label.text = target.health + "/" + target.maxHealth;
    }
    private IEnumerator RecoveryDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isHealing = true;
        StartCoroutine(Heal());
    }

    private IEnumerator Heal()
    {
        yield return new WaitForSeconds(1f);
        target.health += (int)(target.maxHealth * 0.03f);
        if(target.health > target.maxHealth)
        {
            target.health = (int)target.maxHealth;
            StopAllCoroutines();
        }
        StartCoroutine(Heal());
    }
}
