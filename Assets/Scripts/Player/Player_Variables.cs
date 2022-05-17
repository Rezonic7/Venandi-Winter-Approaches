using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Variables : Singleton<Player_Variables>
{
    private Slider healthBar;
    private Slider staminaBar;

    private float maxHealth = 100;
    private float currentHealth;

    private float maxStamina = 100;
    private float currentStamina;

    private WaitForSeconds regentick = new WaitForSeconds(0.1f);

    private Coroutine regenStamina;


    private void Start()
    {
        healthBar = GameObject.FindGameObjectWithTag("PlayerHealth")?.GetComponent<Slider>();
        staminaBar = GameObject.FindGameObjectWithTag("PlayerStamina")?.GetComponent<Slider>();

        if (!staminaBar || !healthBar)
        {
            Debug.Log("Heads up! PlayerVariables will not work, some compenents are missing in the Scene");
            return;
        }
       
        currentStamina = maxStamina;
        currentHealth = maxHealth;

        staminaBar.maxValue = maxStamina;
        healthBar.maxValue = maxHealth;

        staminaBar.value = maxStamina;
        healthBar.value = maxHealth;
    }

    public void TakeDamage(float value)
    {
        if(!healthBar)
        {
            return;
        }
        int armorValue = Player_Equipment.instance.TotalArmor;
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

            Player_Controller.instance.CanRecieveInput = false;
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
        if(!staminaBar)
        {
            return false;
        }
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
