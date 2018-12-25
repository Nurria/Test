using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test<T> : MonoBehaviour where T: MonoBehaviour {

    #region 继承
    //   private bool canLog;
    //   private Coroutine m_coroutine;

    //   public class Bb
    //   {
    //       public string id;

    //       public Bb(string id)
    //       {
    //           Debug.Log("new bb");
    //           this.id = id;
    //       }
    //   }

    //// Use this for initialization
    //void Start () {
    //       Debug.Log("in Test Start Func");

    //       m_coroutine = StartCoroutine(LogFunc());
    //}

    //// Update is called once per frame
    //void Update () {
    //       //Debug.Log("in TEST Update !!  ... ... ");

    //       if (Input.GetMouseButtonDown(1))
    //       {
    //           Debug.Log("mouseButton 1 !!! ... ...");

    //           canLog = true;
    //       }
    //       if (Input.GetMouseButtonUp(1))
    //       {
    //           canLog = false;

    //       }

    //       LogAWord(canLog);


    //   }

    //   protected virtual void LogAWord(bool canLog)
    //   {
    //       if (canLog && m_coroutine != null)
    //       {
    //           Debug.Log("m_coroutine   :  " + m_coroutine);
    //       }
    //   }

    //   private IEnumerator LogFunc()
    //   {
    //       Debug.Log("enumerator start !!! ... ...");
    //       Debug.Log("working !! .. .. ..");
    //       yield return 0;
    //       Debug.Log("LogFunc work over ... ...");
    //   }
    #endregion

    #region 单例
    //protected static TestSon1 _instance;
    //public static TestSon1 Instance
    //{
    //    get
    //    {
    //        if (_instance != null)
    //            return _instance;
    //        _instance = FindObjectOfType<TestSon1>();
    //        if(_instance != null)
    //            return _instance;

    //        Create();
    //        return _instance;
    //    }
    //}

    //private static void Create()
    //{
    //    TestSon1 obj;
    //    try
    //    {
    //        obj = Resources.Load<TestSon1>("doo");
    //    }
    //    catch (Exception e)
    //    {
    //        throw e;
    //    }

    //    if (obj != null)
    //    {
    //        _instance = Instantiate(obj);
    //    }
    //}

    ////private static TestSon1 son1;
    //private bool isClick = false;

    //private void Start()
    //{
    //    if (Instance != null)
    //    {
    //        //Instance.LogAWord(true);
    //    }
    //}

    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        isClick = true;
    //    }
    //    if (Input.GetMouseButtonUp(1))
    //    {
    //        isClick = false;
    //    }
    //}
    #endregion

    public Transform center;

    public static T Instance;

    private void Awake()
    {
        Instance = this as T;
    }

}
