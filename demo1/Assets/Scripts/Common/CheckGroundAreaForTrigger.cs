using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGroundAreaForTrigger : MonoBehaviour {

    [Tooltip("向下检测的距离")]
    public float distances;
    public LayerMask layer;
    public Transform player;
    public CapsuleCollider2D capsule;
    public Rigidbody2D rigid2D;
    public bool isOriginFaceRight;

    private Vector2 m_offset;
    private Vector2 m_diffVec2 = Vector2.zero;
    private Vector2 m_bottomCenter = Vector2.zero;
    private Vector2 pointA;
    private Vector2 pointB;
    private Collider2D result;

    public virtual bool CheckGround()
    {
        m_offset.x = player.localScale.x > 0 && isOriginFaceRight ? capsule.offset.x : -capsule.offset.x;
        m_offset.y = capsule.offset.y * 0.5f;
        m_diffVec2.x = player.localScale.x > 0 && isOriginFaceRight ? capsule.size.x : -capsule.size.x;
        m_diffVec2.y = -(capsule.size.x * 0.5f + distances);
        pointA.x = rigid2D.position.x + m_offset.x + (player.localScale.x > 0 && isOriginFaceRight ? -capsule.size.x * 0.5f : capsule.size.x * 0.5f);
        pointA.y = rigid2D.position.y + m_offset.y - capsule.size.y * 0.5f + capsule.size.x * 0.5f;

        pointB = pointA + m_diffVec2;
        result = Physics2D.OverlapArea(pointA, pointB, layer);
        Debug.DrawLine(rigid2D.position, pointA, Color.yellow);
        Debug.DrawLine(rigid2D.position, pointB, Color.blue);

        return result != null;
    }
}
