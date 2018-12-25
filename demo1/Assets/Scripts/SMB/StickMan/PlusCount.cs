using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlusCount : stickManBase {

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monoBehaviour.PlusCurrentAttackCount();
    }
}
