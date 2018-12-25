using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour {

    public GameObject targetObj;
    public float speed = 5;
    public float offset = 1;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 position = Vector3.Lerp(transform.position, targetObj.transform.position, speed * Time.deltaTime);
        transform.position = new Vector3(position.x, position.y, transform.position.z);
	}
}
