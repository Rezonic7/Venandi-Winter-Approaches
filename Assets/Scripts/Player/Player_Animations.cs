using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Player_Animations : Singleton<Player_Animations>
{
    private Animator anim;
    [SerializeField] private Rig bowWeight;

    private void Start()
    {
        anim = this.GetComponentInChildren<Animator>();
    }

    public void BowStart()
    {
        anim.SetTrigger("Bow_Start");
    }

    public void BowFire()
    {
        anim.SetTrigger("Bow_Fire");
    }
    public void BowDrawn(bool isTrue)
    {
        anim.SetBool("Bow_Drawn", isTrue);
    }
    public void StartAimBow(float weight)
    {
        anim.SetLayerWeight(1, weight);
        bowWeight.weight = weight;
    }

    public void IsAiming(bool isTrue)
    {
        anim.SetBool("isAiming", isTrue);
    }

    public void LightDamage()
    {
        anim.SetTrigger("Hit_Light");
    }

    public void LightClubStart()
    {
        anim.SetTrigger("LC_Start");
      
    }
    public void HeavyClubStart()
    {
        anim.SetTrigger("HC_Start");
    }

    public void WeaponDrawn(bool isTrue)
    {
        anim.SetBool("isWeaponDrawn", isTrue);
    }

    public void SheathWeight(int weight)
    {
        anim.SetLayerWeight(2, weight);
        if(weight == 1)
        {
            anim.SetTrigger("Sheath");
        }
    }

    public void LCDrawn(bool isTrue)
    {
        anim.SetBool("LC_Drawn", isTrue);
    }

    public void HCDrawn(bool isTrue)
    {
        anim.SetBool("HC_Drawn", isTrue);

    }

    public void Walk(bool isTrue)
    {
        anim.SetBool("isWalking", isTrue);
    }

    public void Run(bool isTrue)
    {
        anim.SetBool("isRunning", isTrue);
    }
    public void Roll()
    {
        anim.SetTrigger("Roll");
    }
    public void Gather()
    {
        anim.SetTrigger("Gather");
    }
}
