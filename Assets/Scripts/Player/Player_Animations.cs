using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animations : Singleton<Player_Animations>
{
    private Animator anim;

    
    
    private void Start()
    {
        anim = this.GetComponentInChildren<Animator>();
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
