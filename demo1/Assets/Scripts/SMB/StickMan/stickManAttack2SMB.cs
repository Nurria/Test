using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stickManAttack2SMB : stickManBase
{
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monoBehaviour.CheckFace();
        monoBehaviour.damager.EnableDamage();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monoBehaviour.LockSpeed();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monoBehaviour.damager.DisableDamage();
    }
}
