using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnExitSMB : stickManBase {

    public string[] methods;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        for (int i = 0; i < methods.Length; i++)
        {
            monoBehaviour.Invoke(methods[i], 0f);
        }
    }
}
