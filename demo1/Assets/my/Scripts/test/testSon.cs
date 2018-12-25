using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class testSon : MonoBehaviour {


    #region 无谓
    //private Bb objBb = new Bb("name");
    public bool isClick = false;
    public bool isTranslate = true;

    // Use this for initialization
    //void Start()
    //{

    //}

    //Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isClick = true;
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            isClick = false;
        }
    }

    private void FixedUpdate()
    {
        if (isClick)
        {
            if (isTranslate)
                transform.Translate(new Vector3(10.0f * Time.deltaTime, 0.0f, 0.0f));
            else
                transform.localPosition += new Vector3(10.0f * Time.deltaTime, 0.0f, 0.0f);

        }
    }

    public void LogAWord(bool canLog)
    {
        if (canLog)
        {
            Debug.Log("in testSon");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("I hit something , so I destroy myself !!! ");
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.LogWarning("!!!!!!");
    }

    #endregion

    //private void Start()
    //{
    //    Test<TestSon1>.Instance.LogAWord("str");
    //}

    //public UnityEvent onTest;

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Q))
    //    {
    //        onTest.Invoke();
    //    }
    //}

    //private BoxCollider2D coll;

    //private void Start()
    //{
    //    coll = GetComponentInParent<BoxCollider2D>();
    //    if (coll != null)
    //    {
    //        Debug.LogWarning("Have Coll Component !!! .. .. ");
    //    }
    //}
}
