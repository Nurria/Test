using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;

public class RedKnigtBehaviour : MonoBehaviour {

    
    //Reference
    public Damageable damageable;

    public void Hurt(Damager damager, Damageable damageable)
    {
        Debug.Log("hurt !!!  and the health rest  :  " + damageable.CurrentHealth);
    }

    public void Die(Damager damager, Damageable damageable)
    {
        Debug.Log("die !!! ");
        gameObject.SetActive(false);
    }
}
