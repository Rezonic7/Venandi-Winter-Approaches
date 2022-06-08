using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_AnimalChargeAttack : StateMachineBehaviour
{
    [SerializeField] private float _chargeTime;

    private float Chargetime;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Chargetime = _chargeTime;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AnimalClass animal = animator.gameObject.GetComponent<AnimalClass>();
        if(Chargetime < 0)
        {
            animal.Anim.SetBool("isDoneCharging", true);
        }
        else
        {
            animal.ChargeForward();
            Chargetime -= Time.deltaTime;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    AnimalClass animal = animator.gameObject.GetComponent<AnimalClass>();
    //}

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
