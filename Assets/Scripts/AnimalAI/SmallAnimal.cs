using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SmallAnimal : AnimalClass
{
    private bool hasChosen_AgitateAction;
    public override void Start()
    {
        base.Start();
        IsAgitated = false;
    }
    public override void Update()
    {
        if (IsAgitated)
        {
            if(!hasChosen_AgitateAction)
            {
                int DoRandomAction = Random.Range(0, 2);
                if (DoRandomAction == 0)
                {
                    DoRandomAttack();
                }
                else if (DoRandomAction == 1)
                {
                    MoveToNextArea();
                }
                hasChosen_AgitateAction = true;
            }
        }
        else
        {
            if (IsAttacking)
            {
                return;
            }
            if (IsPlayerInRange)
            {
                Agent.SetDestination(player.transform.position);
                if (Agent.remainingDistance <= AttackRange)
                {
                    Agent.SetDestination(transform.position);
                    DoRandomAttack();
                    IsAttacking = true;
                }
            }
            else
            {
                PassiveState();
            }
        }
    }
    public override void PassiveState()
    {
        if (Agent.remainingDistance <= 0)
        {
            if (LoiterTimer > 0)
            {
                LoiterTimer -= Time.deltaTime;
                Agent.SetDestination(Agent.transform.position);
                Debug.Log("I will be Idle here");
            }
            else
            {
                LoiterTimer = RandomizeTimer(BaseLoiterTime);
                RandomWanderAroundCurrentArea();
                Debug.Log("Im gonna move around here a bit");
            }
        }
    }
    public override void MoveToNextArea()
    {
        base.MoveToNextArea();
        IsAgitated = false;
        hasChosen_AgitateAction = false;
    }
}
