using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpDownSMB : stickManBase
{

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monoBehaviour.CheckFace();
        monoBehaviour.CheckHearyFall();
        monoBehaviour.CheckGround();
        monoBehaviour.GroundMovement();
    }
}
