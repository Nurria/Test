using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearTrigger : StateMachineBehaviour
{
    public string[] Params;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        for (int i = 0; i < Params.Length; i++)
        {
            animator.ResetTrigger(Params[i]);
        }
    }
}
