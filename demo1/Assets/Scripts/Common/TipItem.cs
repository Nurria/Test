using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipItem : MonoBehaviour {

   [Header("--------  配合Interact on Button 2D组件使用  -------")]

    public float height = 1f;
    public Tip.TipType type = Tip.TipType.None;

    bool _showing;

    public bool Showing
    {
        get
        {
            return _showing;
        }
        set
        {
            _showing = value;
            if (_showing)
            {
                Debug.Log("show !!!");
                Tip.Instance.ShowTip(type, transform.position + new Vector3(0, height, 0));
            }
            else
            {
                Debug.Log("hide !!!");
                Tip.Instance.HideTip();
            }
            
        }
    }

    public void Show()
    {
        Showing = true;
    }

    public void Hide()
    {
        Showing = false;   
    }
}
