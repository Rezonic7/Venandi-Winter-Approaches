using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SmallAnimal : AnimalClass
{
    private bool hasChosen_AgitateAction;
   
    public override void Update()
    {
        if(!AreaHolder)
        {
            return;
        }
        if(IsAttacking)
        {
            transform.LookAt(player.gameObject.transform);
            Agent.SetDestination(transform.position);
            return;
        }

        if(IsGoingToNextArea)
        {
            if(Agent.remainingDistance <= 5)
            {
                IsGoingToNextArea = false;
            }
        }


        if (IsAgitated)
        {
            if(!hasChosen_AgitateAction)
            {
                int DoRandomAction = Random.Range(0, 2);
                if (DoRandomAction == 0)
                {
                    DoRandomAttack();
                    IsAgitated = true;
                }
                else if (DoRandomAction == 1)
                {
                    MoveToNextArea();
                }
                hasChosen_AgitateAction = true;
            }
            else
            {
                if (IsPlayerInRange)
                {
                    Agent.SetDestination(player.transform.position);
                    if (Agent.remainingDistance >= AttackRange)
                    {
                        Agent.SetDestination(transform.position);
                        DoRandomAttack();
                    }
                }
                else
                {
                    PassiveState();
                }
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
        if (Agent.remainingDistance <= 3)
        {
            if (LoiterTimer > 0)
            {
                LoiterTimer -= Time.deltaTime;
                Agent.SetDestination(Agent.transform.position);
                Anim.SetBool("isWalking", false);
                Debug.Log("I will be Idle here");
            }
            else
            {
                LoiterTimer = RandomizeTimer(BaseLoiterTime);
                Anim.SetBool("isWalking", true);
                RandomWanderAroundCurrentArea();
                Debug.Log("Im gonna move around here a bit");
            }
        }
    }
   
    public override void MoveToNextArea()
    {
        base.MoveToNextArea();
    }
}
