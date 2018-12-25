using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnterSMB : stickManBase {

    public string[] methods;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        for (int i = 0; i < methods.Length; i++)
        {
            monoBehaviour.Invoke(methods[i], 0f);
        }
    }
}
