using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainingDummy : MonoBehaviour
{
    private Slider healthBar;
    private float currentHealth;
    private Animator anim;

    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float respawnTime = 5;
    private void Start()
    {
        healthBar = GetComponentInChildren<Slider>();
        anim = GetComponent<Animator>();

        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
    }

    public void TakeDamage(float value)
    {
        if (currentHealth - value >= 0)
        {
            currentHealth -= value;
            healthBar.value = currentHealth;
            anim.SetTrigger("TakeDamage");
        }
        else
        {
            currentHealth = 0;
            healthBar.value = currentHealth;

            anim.SetBool("isDead", true);
            healthBar.gameObject.SetActive(false);
            StartCoroutine(Respawn());
        }
    }
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);
        healthBar.gameObject.SetActive(true);
        currentHealth = maxHealth;
        healthBar.value = currentHealth;
        anim.SetBool("isDead", false);
    }

    
}
