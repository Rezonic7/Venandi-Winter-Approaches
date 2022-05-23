using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SmallAnimal : AnimalClass
{
    private Collider hurtBox;
    public override void Start()
    {
        base.Start();
        hurtBox = transform.GetChild(0).GetChild(0).Find("HurtBox").GetComponent<Collider>();

        hurtBox.enabled = false;
    }
    public override void CalmState()
    {
        if (Agent.remainingDistance <= 3)
        {
            if (LoiterTimer > 0)
            {
                LoiterTimer -= Time.deltaTime;
                Agent.SetDestination(Agent.transform.position);
            }
            else
            {
                LoiterTimer = RandomizeTimer(BaseLoiterTime);
                RandomWanderAroundCurrentArea();
            }
        }
    }
    public void SetHurtBox(int isTrue)
    {
        if(isTrue <= 0)
        {
            hurtBox.enabled = false;
        }else
        {
            hurtBox.enabled = true;
        }
    }
    

}
