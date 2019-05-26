using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectDetail
{
    Size,
    IsTrigger,
    UseEffector,
    All
}

public enum PointEffector2DDetail
{
    ForceMagnitude,
    ForceVariation,
    DistanceScale,
    All
}

[RequireComponent(typeof(LineRenderer))]
[ExecuteInEditMode]
public class rope : MonoBehaviour {

    [Header(" -----------  基本参数设置  -----------")]
    public float constDistance;
    public float collSize;
    public bool isTrigger;
    public bool useEffector;
    public bool maxDistanceOnly;
    public float gravity;
    public float forceMag;
    public float forceVar;
    public float distanceScale;

    [Header(" -----------  编辑形状参数  -----------")]
    public float speed;


    LineRenderer _lineRenderer;
    DistanceJoint2D[] _dj2Ds;
    RopeNode2D[] _nodes;
    Rigidbody2D[] _rigid2Ds;
    Vector3[] _points;
    BoxCollider2D[] _box2Ds;
    
    RopeNode2D _start;
    RopeNode2D _behindStart;
    RopeNode2D _beforEnd;
    RopeNode2D _end;

    //Collider2D
    float _currentColliderSize;
    bool _curCollTriggerState;
    bool _curCollUseEffectorState;
    
    //RigidBody2D
    float _curGravity;

    //DistanceJoint2D
    bool _curMaxDistanceOnly;

    //PointEffector2D
    float _curForceMag;
    float _curForceVar;
    float _curDistanceScale;

    float _totalLenSqr;                 //绳子总长平方
    int _curNodesCount;

    //edit时用的变量
    Vector3 targetPos;
    float realDis;                      //实际距离
    float x;
    float y;

    void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    // Use this for initialization
    void Start () {
        InitValues();

        SetNodes(true);

    }

    void InitValues()
    {
        _currentColliderSize = collSize;
        _curCollTriggerState = isTrigger;
        _curCollUseEffectorState = useEffector;

        _curMaxDistanceOnly = maxDistanceOnly;

        _curGravity = gravity;

        _curForceMag = forceMag;
        _curForceVar = forceVar;
        _curDistanceScale = distanceScale;

        _curNodesCount = 0;
    }
	
	// Update is called once per frame
	void Update () {
#if UNITY_EDITOR
        SetNodes();
        SetCollider();
        SetColliderDetial(CollectDetail.All);
        SetDistanceJoint2D();
        SetDistanceJoint2DDetail();
        SetRigid2D();
        SetPointEffector2D(PointEffector2DDetail.All);
        if (!Application.isPlaying)
        {
            EditShape();
        }
#endif
        LockMaxDistance();
        UpdateRenderer();
    }

    void SetNodes(bool force = false)
    {
        //_nodes包含当前节点下的所有子节点，包括_start和_end
        if (_nodes == null || _curNodesCount != transform.childCount || force)
        {
            var childer = GetComponentsInChildren<Transform>();
            _nodes = new RopeNode2D[childer.Length - 1];
            if (_nodes.Length < 4)
            {
                throw new UnityException("节点个数不能少于4个");
            }
            _points = new Vector3[_nodes.Length - 2];
            _totalLenSqr = Mathf.Pow(constDistance * (_nodes.Length - 1), 2);

            if (_beforEnd != null && _beforEnd.connectToEndPoint)
            {
                _beforEnd.DestroyConnectToEnd();
            }
            for (int i = 0; i < _nodes.Length; i++)
            {
                _nodes[i] = new RopeNode2D(childer[i + 1]);
            }

            for (int i = 0; i < _nodes.Length; i++)
            {
                _start = _nodes[0];
                _behindStart = _nodes[1];
                _beforEnd = _nodes[_nodes.Length - 2];
                _end = _nodes[_nodes.Length - 1];
                if (_nodes[i] == _start)
                {
                    _start.Rigid2D.bodyType = RigidbodyType2D.Kinematic;
                    _start.Rigid2D.constraints = RigidbodyConstraints2D.FreezeRotation;

                    _start.DestroyRestComponent(PointType.Start);
                }
                else if (_nodes[i] == _end)
                {
                    _end.Rigid2D.bodyType = RigidbodyType2D.Kinematic;
                    _end.DestroyRestComponent(PointType.End);
                    _end.Rigid2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                }

                if (_nodes[i] != _start && _nodes[i] != _end)
                {
                    if (!_nodes[i].pointEffector2D)
                        _nodes[i].pointEffector2D = _nodes[i].gameObject.AddComponent<PointEffector2D>();
                    if (!_nodes[i].dj2D)
                        _nodes[i].dj2D = _nodes[i].gameObject.AddComponent<DistanceJoint2D>();

                    _nodes[i].Rigid2D.bodyType = RigidbodyType2D.Dynamic;
                    _nodes[i].dj2D.connectedBody = _nodes[i - 1].Rigid2D;
                    _nodes[i].dj2D.autoConfigureConnectedAnchor = false;
                    _nodes[i].dj2D.anchor = Vector2.zero;
                    _nodes[i].dj2D.connectedAnchor = Vector2.zero;
                    _nodes[i].dj2D.autoConfigureDistance = false;

                    if (_nodes[i] == _behindStart)
                    {
                        _nodes[i].dj2D.distance = 0.005f;
                    }
                    else if (_nodes[i] == _beforEnd)
                    {
                        _beforEnd.dj2D.distance = constDistance;
                        
                        _beforEnd.ConnectToEnd(_end.Rigid2D);
                    }
                    else
                        _nodes[i].dj2D.distance = constDistance;
                }
            }
            _curNodesCount = transform.childCount;

            SetCollider(force);
            SetColliderDetial(CollectDetail.All, force);
            SetDistanceJoint2D(force);
            SetDistanceJoint2DDetail(force);
            SetRigid2D(force);
            SetPointEffector2D(PointEffector2DDetail.All, force);
        }
    }

    void SetCollider(bool force = false)
    {
        if (_box2Ds == null || force)
        {
            _box2Ds = new BoxCollider2D[_nodes.Length - 2];
            for (int i = 0; i < _nodes.Length; i++)
            {
                if (i == 0 || i == _nodes.Length - 1)
                    continue;

                _box2Ds[i - 1] = _nodes[i].boxColl2D;
            }
        }
    }

    void SetColliderDetial(CollectDetail detail, bool force = false)
    {
        switch (detail)
        {
            case CollectDetail.Size:
                if (_currentColliderSize != collSize || force)
                {
                    for (int i = 0; i < _nodes.Length; i++)
                    {
                        if (_nodes[i].boxColl2D)
                        {
                            _nodes[i].boxColl2D.size = new Vector2(collSize, collSize);
                        }
                    }
                    _currentColliderSize = collSize;
                }
                break;
            case CollectDetail.IsTrigger:
                if (_curCollUseEffectorState != useEffector || force)
                {
                    for (int i = 0; i < _nodes.Length; i++)
                    {
                        if (_nodes[i].boxColl2D)
                        {
                            _nodes[i].boxColl2D.usedByEffector = useEffector;
                        }
                    }
                    _curCollUseEffectorState = useEffector;
                }
                break;
            case CollectDetail.UseEffector:
                if (_curCollTriggerState != isTrigger || force)
                {
                    for (int i = 0; i < _nodes.Length; i++)
                    {
                        if (_nodes[i].boxColl2D)
                        {
                            _nodes[i].boxColl2D.isTrigger = isTrigger;
                        }
                    }
                    _curCollTriggerState = isTrigger;
                }
                break;
            default:
                SetColliderDetial(CollectDetail.Size, force);
                SetColliderDetial(CollectDetail.IsTrigger, force);
                SetColliderDetial(CollectDetail.UseEffector, force);
                break;
        }
    }

    void SetDistanceJoint2D(bool force = false)
    {
        if (_dj2Ds == null || force)
        {
            _dj2Ds = new DistanceJoint2D[_nodes.Length - 2];
            for (int i = 0; i < _nodes.Length; i++)
            {
                if (i == 0 || i == _nodes.Length - 1)
                    continue;

                _dj2Ds[i - 1] = _nodes[i].dj2D;
            }
        }
    }

    void SetDistanceJoint2DDetail(bool force = false)
    {
        //目前只需要控制maxDistanceOnly
        if (_curMaxDistanceOnly != maxDistanceOnly || force)
        {
            for (int i = 0; i < _nodes.Length; i++)
            {
                if (i == 0 || i == _nodes.Length - 1)
                    continue;

                _nodes[i].dj2D.maxDistanceOnly = maxDistanceOnly;
                _curMaxDistanceOnly = maxDistanceOnly;
            }
        }
    }

    void SetRigid2D(bool force = false)
    {
        if (_curGravity != gravity || force)
        {
            for (int i = 0; i < _nodes.Length; i++)
            {
                if (i == 0 || i == _nodes.Length - 1)
                    continue;

                _nodes[i].Rigid2D.gravityScale = gravity;
                _curGravity = gravity;
            }
        }
    }

    void SetPointEffector2D(PointEffector2DDetail detail, bool force = false)
    {
        switch (detail)
        {
            case PointEffector2DDetail.ForceMagnitude:
                if (_curForceMag != forceMag || force)
                {
                    for (int i = 0; i < _nodes.Length; i++)
                    {
                        if (_nodes[i].pointEffector2D)
                        {
                            _nodes[i].pointEffector2D.forceMagnitude = forceMag;
                        }
                    }
                    _curForceMag = forceMag;
                }
                break;
            case PointEffector2DDetail.ForceVariation:
                if (_curForceVar != forceVar || force)
                {
                    for (int i = 0; i < _nodes.Length; i++)
                    {
                        if (_nodes[i].pointEffector2D)
                        {
                            _nodes[i].pointEffector2D.forceVariation = forceVar;
                        }
                    }
                    _curForceVar = forceVar;
                }
                break;
            case PointEffector2DDetail.DistanceScale:
                if (_curDistanceScale != distanceScale || force)
                {
                    for (int i = 0; i < _nodes.Length; i++)
                    {
                        if (_nodes[i].pointEffector2D)
                        {
                            _nodes[i].pointEffector2D.distanceScale = distanceScale;
                        }
                    }
                    _curDistanceScale = distanceScale;
                }
                break;
            default:
                SetPointEffector2D(PointEffector2DDetail.ForceMagnitude, force);
                SetPointEffector2D(PointEffector2DDetail.ForceVariation, force);
                SetPointEffector2D(PointEffector2DDetail.DistanceScale, force);
                break;
        }
    }

    void EditShape()
    {
        for (int i = 0; i < _nodes.Length; i++)
        {
            //_points[i] = _nodes[i].transform.position;
            if (i == 0)
            {
                _nodes[i + 1].transform.position = _start.transform.position;
                continue;
            }
            else if(_nodes[i] == _end)
            {
                _nodes[i].transform.position = _beforEnd.transform.position;
                continue;
            }
            
            _nodes[i + 1].transform.position = KeepDistance(_nodes[i].transform.position, _nodes[i + 1].transform.position);
        }
    }
    
    //线条的渲染
    void UpdateRenderer()
    {
        if (_points == null)
            return;

        for (int i = 0; i < _nodes.Length - 2; i++)
        {
            _points[i] = _nodes[i + 1].transform.position;
        }

        if (_points.Length != _lineRenderer.positionCount)
        {
            _lineRenderer.positionCount = _points.Length;
        }
        _lineRenderer.SetPositions(_points);
    }

    //编辑状态下的节点位置修正   通过当前节点对后面一个节点的位置修正
    private Vector3 KeepDistance(Vector3 p1, Vector3 p2)
    {
        realDis = Vector3.Magnitude(p1 - p2);
        if (realDis * realDis > constDistance * constDistance)
        {
            x = (p2.x - p1.x) * constDistance / realDis;
            y = (p2.y - p1.y) * constDistance / realDis;
            targetPos.x = p1.x + x; targetPos.y = p1.y + y; targetPos.z = p1.z;
            return Vector3.Lerp(p2, targetPos, constDistance / realDis * speed);
        }
        return p2;
    }

    void LockMaxDistance()
    {
        if (Vector3.SqrMagnitude(_start.transform.position - _end.transform.position) > _totalLenSqr)
        {
            //超过了总长
            Debug.Log("超过了总长");
            
        }
    }
}

public enum PointType
{
    Start,
    End
}

class RopeNode2D
{
    public GameObject gameObject;
    public Transform transform;
    public BoxCollider2D boxColl2D;
    public DistanceJoint2D dj2D;
    public PointEffector2D pointEffector2D;
    public DistanceJoint2D connectToEndPoint;

    public bool isOutPoint;
    public bool isFixed;

    Rigidbody2D rigid2D;
    public Rigidbody2D Rigid2D
    {
        get
        {
            if (!rigid2D)
                rigid2D = gameObject.AddComponent<Rigidbody2D>();
            return rigid2D;
        }
        private set
        {
            rigid2D = value;
        }
    }

    public RopeNode2D(Transform transform)
    {
        this.transform = transform;
        gameObject = transform.gameObject;
        rigid2D = gameObject.GetComponent<Rigidbody2D>();
        boxColl2D = gameObject.GetComponent<BoxCollider2D>();
        dj2D = gameObject.GetComponent<DistanceJoint2D>();
        pointEffector2D = gameObject.GetComponent<PointEffector2D>();
    }

    public void ConnectToEnd(Rigidbody2D rigid)
    {
        var joints = transform.GetComponents<DistanceJoint2D>();
        if (joints.Length > 1)
        {
            connectToEndPoint = joints[1];
        }
        else
        {
            connectToEndPoint = gameObject.AddComponent<DistanceJoint2D>();
        }
        connectToEndPoint.connectedBody = rigid;
        connectToEndPoint.autoConfigureConnectedAnchor = false;
        connectToEndPoint.autoConfigureDistance = false;
        connectToEndPoint.connectedAnchor = Vector2.zero;
        connectToEndPoint.anchor = Vector2.zero;
        connectToEndPoint.distance = 0.005f;
    }

    public void DestroyConnectToEnd()
    {
        MonoBehaviour.DestroyImmediate(connectToEndPoint);
        connectToEndPoint = null;
    }

    public void DestroyRestComponent(PointType type)
    {
        if (type == PointType.Start)
        {
            MonoBehaviour.DestroyImmediate(boxColl2D);
            boxColl2D = null;
        }
        MonoBehaviour.DestroyImmediate(pointEffector2D);
        MonoBehaviour.DestroyImmediate(dj2D);
        pointEffector2D = null;
        dj2D = null;
    }
}
