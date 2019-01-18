using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tip : MonoBehaviour
{
    static Tip _instance;
    public static Tip Instance
    {
        get
        {
            if (_instance != null)
                return _instance;

            _instance = FindObjectOfType<Tip>();
            if (_instance != null)
                return _instance;

            Create();

            return _instance;
        }
    }

    static void Create()
    {
        var go = Instantiate(Resources.Load<GameObject>("TipController")) as GameObject;
        if (go != null)
        {
            _instance = go.GetComponent<Tip>();
        }

        go = new GameObject("TipController");
        _instance = go.AddComponent<Tip>();
    }

    public GameObject Interact;
    public GameObject Attack;
    public GameObject Dash;

    Dictionary<TipType, GameObject> Tips = new Dictionary<TipType, GameObject>();
    GameObject _currentTip;

    public enum TipType
    {
        None,
        Interact,
        Attack,
        Dash
    }

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {

    }

    public void ShowTip(TipType type, Vector3 position)
    {
        HideTip();
        _currentTip = Load(type);
        _currentTip.transform.position = position;
        _currentTip.SetActive(true);
    }

    private GameObject Load(TipType type)
    {
        if (Tips.ContainsKey(type))
        {
            return Tips[type];
        }

        string src = null;
        switch (type)
        {
            case TipType.Interact:
                src = "Prefabs/Button_E";
                break;
            case TipType.Attack:
                src = "Prefabs/Button_J";
                break;
            case TipType.Dash:
                src = "Prefabs/Button_L";
                break;
            default:
                Debug.LogWarning("no this tip type !!!  ");
                break;
        }
        var go = Instantiate(Resources.Load<GameObject>(src));
        go.transform.SetParent(gameObject.transform);
        Tips.Add(type, go);
        return go;
    }

    public void HideTip()
    {
        if (_currentTip == null)
            return;

        _currentTip.SetActive(false);
        _currentTip = null;
    }

    public void HideAllTip()
    {

    }
}
