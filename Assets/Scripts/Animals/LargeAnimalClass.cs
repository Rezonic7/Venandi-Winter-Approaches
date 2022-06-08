using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeAnimalClass : AnimalClass
{
    [Header("Seconds")]
    [SerializeField] private float _combatTime;
    private float combatTimer;
    private bool isInCombat;

    private bool firstEscapeTrigger;
    private bool secondEscapeTrigger;
    private bool thirdEscapeTrigger;

    private int noOfAttacks;

    public override void Start()
    {
        base.Start();

        isInCombat = false;
        combatTimer = _combatTime;
        noOfAttacks = 2;

        firstEscapeTrigger = false;
        secondEscapeTrigger = false;
        thirdEscapeTrigger = false;
    }
    public override void Update()
    {
        base.Update();
        if (isInCombat)
        {
            StartCombatTimer();
        }
        else
        {
            combatTimer = _combatTime;
        }


        if (CurrentHealth < MaxHealth * 0.15)
        {
            noOfAttacks = 6;

        }
        else if(CurrentHealth < MaxHealth * 0.30)
        {
            noOfAttacks = 5;

        }
        else if (CurrentHealth < MaxHealth * 0.50)
        {
            noOfAttacks = 4;
        }
        else if (CurrentHealth < MaxHealth * 0.75)
        {
            noOfAttacks = 3;
        }
    }
    public override void AggressiveState()
    {
        base.AggressiveState();
        isInCombat = true;
    }
    public override void CalmState()
    {
        base.CalmState();
        isInCombat = false;
    }
    public override void DoRandomAttack()
    {
        HasDamaged = false;

        int RandomAttack = Random.Range(0, (noOfAttacks));
        if (RandomAttack == 0)
        {
            Anim.SetTrigger("Attack1");
        }
        else if (RandomAttack == 1)
        {
            Anim.SetTrigger("Attack2");
        }
        else if (RandomAttack == 2)
        {
            Anim.SetTrigger("Attack3");
        }
        else if (RandomAttack == 3)
        {
            Anim.SetTrigger("Attack4");
        }
        else if (RandomAttack == 4)
        {
            Anim.SetTrigger("Combo1");
        }
        else if (RandomAttack == 5)
        {
            Anim.SetTrigger("Combo2");
        }
        IsAttacking = true;
        CanMove = false;
    }
   
    public override void TakeDamage(int value)
    {
        base.TakeDamage(value);
        if(CurrentHealth < (MaxHealth * 0.15f))
        {
            if (!thirdEscapeTrigger)
            {
                MoveToNextArea();
                thirdEscapeTrigger = true;
            }
        }
        else if (CurrentHealth < (MaxHealth * 0.5f))
        {
            if(!secondEscapeTrigger)
            {
                MoveToNextArea();
                secondEscapeTrigger = true;
            }
        }
        else if(CurrentHealth < (MaxHealth * 0.75f))
        {
            if (!firstEscapeTrigger)
            {
                MoveToNextArea();
                firstEscapeTrigger = true;
            }
        }
    }
    void StartCombatTimer()
    {
        if(combatTimer <= 0)
        {
            MoveToNextArea();
            if (CurrentHealth < ((MaxHealth * 0.15f) * 1.1f))
            {
                thirdEscapeTrigger = true;
            }
            else if (CurrentHealth < ((MaxHealth * 0.5f) * 1.1f))
            {
                secondEscapeTrigger = true;
            }
            else if (CurrentHealth < ((MaxHealth * 0.75f) * 1.1f))
            {
                firstEscapeTrigger = true;
            }
            isInCombat = false;
        }
        else
        {
            combatTimer -= Time.deltaTime;
        }    
    }
}
