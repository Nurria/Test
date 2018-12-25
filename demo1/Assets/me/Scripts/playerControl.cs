using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControl : MonoBehaviour {

    public float m_speed = 5;
    public float runSpeed = 10;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float h = Input.GetAxis("Horizontal");
        if(Mathf.Abs(h) > 0.1)
        {
            Debug.Log("is press a/d");
        }
        transform.position += new Vector3(h, 0, 0) * Time.deltaTime * m_speed;

        if (Input.GetKeyDown(KeyCode.L))
        {
            //播放快跑动画

        }
	}

    
}
