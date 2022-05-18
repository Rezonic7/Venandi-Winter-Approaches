using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SmallAnimal : AnimalClass
{
    public override void CalmState()
    {
        if (Agent.remainingDistance <= 3)
        {
            if (LoiterTimer > 0)
            {
                LoiterTimer -= Time.deltaTime;
                Agent.SetDestination(Agent.transform.position);
                Anim.SetBool("isWalking", false);
            }
            else
            {
                LoiterTimer = RandomizeTimer(BaseLoiterTime);
                Anim.SetBool("isWalking", true);
                RandomWanderAroundCurrentArea();
            }
        }
    }
}
