using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Variables : Singleton<Player_Variables>
{
    public Slider healthBar;
    public Slider staminaBar;

    private float maxHealth = 100;
    private float currentHealth;

    private float maxStamina = 100;
    private float currentStamina;

    private WaitForSeconds regentick = new WaitForSeconds(0.1f);

    private Coroutine regenStamina;

    private void Start()
    {
        currentStamina = maxStamina;
        currentHealth = maxHealth;

        staminaBar.maxValue = maxStamina;
        healthBar.maxValue = maxHealth;

        staminaBar.value = maxStamina;
        healthBar.value = maxHealth;
    }

    public void TakeDamage(float value)
    {
        int armorValue = Player_Equipment.instance.totalArmor;
        if (value - armorValue <= 0)
        {
            value = 1;
        }
        else
        {
            value = value - armorValue;
        }
        Debug.Log(value);
        if (currentHealth - value >= 0)
        {
            currentHealth -= value;
            healthBar.value = currentHealth;

            Player_Controller.instance.canRecieveInput = false;
            Player_Animations.instance.LightDamage();  
        }
        else
        {
            currentHealth = 0;
            healthBar.value = currentHealth;
            Debug.Log("Player Died");
        }
    }

    public bool UseStamina(float value)
    {
        if(currentStamina - value >= 0)
        {
            currentStamina -= value;
            staminaBar.value = currentStamina;

            if(regenStamina != null)
            StopCoroutine(regenStamina);

            regenStamina = StartCoroutine(RegenStamina());

            return true;
        }else
        {
            return false;
        }
    }

    IEnumerator RegenStamina()
    {
        yield return new WaitForSeconds(1);

        while(currentStamina < maxStamina)
        {
            currentStamina += maxStamina / 100;
            staminaBar.value = currentStamina;
            yield return regentick;
        }
        regenStamina = null;
    }
}
