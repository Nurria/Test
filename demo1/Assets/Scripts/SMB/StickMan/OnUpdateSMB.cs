using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnUpdateSMB : stickManBase {

    public string[] methods;

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        for (int i = 0; i < methods.Length; i++)
        {
            monoBehaviour.Invoke(methods[i], 0f);
        }
    }
}
