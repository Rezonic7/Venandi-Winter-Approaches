using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SmallAnimalClass : AnimalClass
{
    public override void Start()
    {
        base.Start();
    }
    public override void CalmState()
    {
        if(IsAttacking)
        {
            return;
        }
        if (Agent.remainingDistance <= 3)
        {
            if (LoiterTimer > 0)
            {
                LoiterTimer -= Time.deltaTime;
                Agent.SetDestination(Agent.transform.position);
                AgentHasPath = false;
            }
            else
            {
                LoiterTimer = RandomizeTimer(BaseLoiterTime);
                RandomWanderAroundCurrentArea();
            }
        }
        if (IsAggressive)
        {
            RandomCalmAnimation();
            IsAggressive = false;
        }
    }
   
    

}
