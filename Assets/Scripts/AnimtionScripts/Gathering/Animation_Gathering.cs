using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Gathering : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ItemClass item = null;
        int amount = 0;
        GatheringSpots GS = Player_Controller.instance.GatheringItem?.GetComponent<GatheringSpots>();
        
        if(!GS)
        {
            Debug.Log("I am Carving");
            CarvingSpot CS = Player_Controller.instance.CavingSpot?.GetComponent<CarvingSpot>();
            Debug.Log(CS);
            ItemDropsClass dcItem = CS?.GatherItem();
            Debug.Log(dcItem);
            item = dcItem.Item;
            Debug.Log("I got "+ dcItem.Item);
            amount = Random.Range(dcItem.MinQuantityAmount, dcItem.MaxQuantityAmount + 1);
        }
        else
        {
            GatherableItems gsItem = GS?.GatherItem();
            item = gsItem.Item;
            amount = Random.Range(gsItem.MinAmount, gsItem.MaxAmount + 1);
        }
        
        if (amount <= 0)
        {
            CanvasManager.instance.ShowInfo("You Got Nothing");
            return;
        }
        if(item)
        {
            MissionManager.instance.UpdateGatherGoal(item, amount);
            Player_Inventory.instance.AddItem(item, amount);
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
