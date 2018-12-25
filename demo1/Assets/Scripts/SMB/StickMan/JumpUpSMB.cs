using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpUpSMB : stickManBase
{

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monoBehaviour.CheckFace();
        if (monoBehaviour.CheckJump())
        {
            monoBehaviour.Jump();
        }
        monoBehaviour.CheckCeil();
        monoBehaviour.CheckGround();
        monoBehaviour.GroundMovement();
    }

    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    monoBehaviour.CancelJumpSignal();
    //}
}
