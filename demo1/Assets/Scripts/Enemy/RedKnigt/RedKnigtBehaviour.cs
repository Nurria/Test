using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;

public class RedKnigtBehaviour : MonoBehaviour {

    
    //Reference
    public Damageable damageable;
    public PlatformEffector2D effector2D;
    private Animator anim;

    int m_DieState = Animator.StringToHash("die");

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Hurt(Damager damager, Damageable damageable)
    {
        Debug.Log("hurt !!!  and the health rest  :  " + damageable.CurrentHealth);
    }

    public void Die(Damager damager, Damageable damageable)
    {
        Debug.Log("die !!! ");
        anim.SetTrigger(m_DieState);
        //gameObject.SetActive(false);
        CloseLayerMask("Player");
    }

    public void BeFinalKIll(FinalKill damager, Damageable damageable)
    {
        Debug.Log("beFinalKill !!! ");
        anim.SetTrigger(m_DieState);        //暂时为死亡状态，本应是倒地之类的
        if (damageable.CurrentHealth <= 0)
        {
            CloseLayerMask("Player");
        }
    }

    private void CloseLayerMask(string layerName)
    {
        int layerMask = 1 << LayerMask.NameToLayer(layerName);
        effector2D.colliderMask &= ~layerMask;
    }

    private void OpenLayerMask(string layerName)
    {
        int layerMask = 1 << LayerMask.NameToLayer(layerName);
        effector2D.colliderMask |= layerMask;
    }
}
//testMerge