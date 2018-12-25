using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSon1 : Test<TestSon1> {

    #region 无谓
    //   public Hashtable resourcesTable;

    //   private GameObject doo;

    //   // Use this for initialization
    //   void Start () {

    //       resourcesTable = new Hashtable();
    //       doo = LoadObj("m_Prefabs/doo");
    //   }

    //   //private void OnEnable()
    //   //{

    //   //}

    //   // Update is called once per frame
    //   void Update () {
    //       if (Input.GetKeyUp(KeyCode.F))
    //       {
    //           Debug.Log("press F");
    //           if (doo != null)
    //           {
    //               Debug.Log("Destroy doo");
    //               Destroy(doo);
    //           }
    //       }
    //       else if (Input.GetKeyUp(KeyCode.C))
    //       {
    //           Debug.Log("press C");
    //           if (doo == null)
    //           {
    //               Debug.Log("LoadObj");
    //               doo = LoadObj("m_Prefabs/doo.prefab", true);
    //           }
    //       }
    //}

    //   public GameObject LoadObj(string path, bool cathe = false)
    //   {
    //       Debug.Log("LoadObj Func !!! .... ");
    //       GameObject obj;
    //       if (resourcesTable.ContainsKey(path))
    //       {
    //           obj = resourcesTable[path] as GameObject; 
    //       }
    //       else
    //       {
    //           obj = Resources.Load(path) as GameObject;
    //           if (cathe)
    //           {
    //               resourcesTable.Add(path, obj);
    //           }
    //       }

    //       return Instantiate(obj);
    //   }
    #endregion

    #region 测试碰撞器主动方在进入触发器内部后被销毁是否会出发OnTriggerExit2D方法,,  答案是会被触发
    ////被撞的一方

    //public void LogAWord(string str)
    //{
    //    Debug.Log(str);
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Debug.LogWarning("Something enter my Trigger");
    //}

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    Debug.Log("is Stay in Trigger");
    //    Destroy(collision.gameObject);
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    Debug.LogWarning("Something Enter and Exit My Trigger");
    //}
    #endregion

    private MeshFilter m_filter;
    private MeshRenderer m_renderer;

    public SortingLayer sortingLayer;

    private void Start()
    {
        m_filter = GetComponent<MeshFilter>();
        m_renderer = GetComponent<MeshRenderer>();

        drawMesh();
    }

    private void drawMesh()
    {
        Mesh mesh = new Mesh();
        mesh.name = "triangleMesh";
        m_filter.mesh = mesh;

        Vector3[] pts = new Vector3[3];
        int[] triangles = new int[3];

        pts[0].Set(0, 0, 0);
        pts[1].Set(0, 1, 0);
        pts[2].Set(1, 0, 0);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        mesh.Clear();

        mesh.vertices = pts;
        mesh.triangles = triangles;

        Debug.Log("不上传");
        //mesh.UploadMeshData(false);
    }
}
