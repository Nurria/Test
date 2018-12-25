﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocoMotionSMB : stickManBase{

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monoBehaviour.CancelJumpSignal();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monoBehaviour.CheckFace();
        monoBehaviour.CheckGround();
        monoBehaviour.GroundMovement();
        monoBehaviour.CheckJump();
    }
}
