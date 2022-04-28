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
        GatheringSpots GS = Player_Controller.instance.gatheringItem?.GetComponent<GatheringSpots>();
        GatherableMaterialData item = GS?.GatherItem();
        int amount = Random.Range(item.MinAmount, item.MaxAmount + 1);

        Debug.Log(item.Item.Name + amount);
        if (item.Item.Name == "Nothing")
        {
            CanvasManager.instance.ShowInfo("You Got Nothing", 5f);
            return;
        }
        if (amount <= 0)
        {
            CanvasManager.instance.ShowInfo("You Got Nothing", 5f);
            return;
        }
        Player_Inventory.instance.AddItem(item.Item, amount);
        CanvasManager.instance.ShowInfo("You have recieved " + item.Item.Name + " x" + amount + "!", 5f);
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
