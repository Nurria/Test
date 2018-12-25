﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gamekit2D;
using System;

public class RoleContr : MonoBehaviour
{
    [Header("-----------  Move  -----------")]
    public float xForce = 5;
    public float vxMax = 3;
    private float m_Horizontal = 0f;

    [Header("-----------  Dash  -----------")]
    private bool isDash = false;
    public float xDashForce = 5;
    public float vxDashMax = 3;
    private float m_Vx_Max;

    [Header("-----------  Jump  -----------")]
    public float ySpeed = 10;
    public float JumpForceCountTotle = 0.4f;
    [Tooltip("是否在跳跃的过程中使用摩擦力为0的材质，如果是，请添加材质引用")]
    public bool useFZero = false;
    private float JumpForceCount;

    [Header("-----------  Attack  -----------")]
    public float Attack1_Movement = 1;
    public float AttackWaitTime = 1f;
    public int AttackCountTotal = 0;
    private int m_CurrentAttackCount = 0;
    private IEnumerator m_WaitNextAttack = null;

    private Vector3 dir;
    private Vector3 temp;
    private Vector2 force = new Vector2(0, 0);
    private Vector2 velo = new Vector2(0, 0);

    [Header("-----------  Reference  -----------")]
    public Damager damager;
    public GameObject trailEffect;
    public CapsuleCollider2D capsuleColl;
    public PhysicsMaterial2D FZero;
    public HearyFall hearyFall;
    private CharacterController2D m_characterController2D;
    private Animator m_anim;
    private Rigidbody2D m_rigid;

    private bool isGround = false;
    private bool isJump = false;
    private bool isAttack = false;
    private bool isSit = false;
    private int attackInt = 0;
    
    [Header("-----------  Naili  -----------")]
    public Slider nailiSlider;
    public float nailiReduceRate = 5;
    public float nailiMax = 100;

    private int m_VxState = Animator.StringToHash("Vx");
    private int m_VyState = Animator.StringToHash("Vy");
    private int m_IsJumpState = Animator.StringToHash("isJump");
    private int m_IsGroundState = Animator.StringToHash("isGround");
    private int m_IsSitState = Animator.StringToHash("isSit");
    private int m_AttatckState = Animator.StringToHash("attackInt");
    private int m_AttatckTriggerState = Animator.StringToHash("attack");
    private int m_HearyFallTriggerState = Animator.StringToHash("hearyFall");


    // Use this for initialization
    void Awake()
    {
        InitReference();
    }

    void Start()
    {
        ResetJumpForceCount();
    }

    private void Update()
    {
        GetDashInput();
        GetSitInput();
        GetAttackInput();
    }

    void FixedUpdate()
    {
        Move();
        CtrlNailiValue();

        //Attack();             //用AttackInt
        AttackByTrigger();      //用SetTrigger

        SetAnimValue();
    }

    private void SetAnimValue()
    {
        m_anim.SetFloat(m_VyState, m_rigid.velocity.y);
        m_anim.SetBool(m_IsSitState, isSit);
    }

    private void Move()
    {
        m_rigid.AddForce(force);
        if (Mathf.Abs(m_rigid.velocity.x) > m_Vx_Max)
        {
            velo.x = dir.x > 0 ? m_Vx_Max : -m_Vx_Max;
            velo.y = m_rigid.velocity.y;
            m_rigid.velocity = velo;
        }
        m_anim.SetFloat(m_VxState, Mathf.Abs(m_rigid.velocity.x));
        force = Vector2.zero;

        //force.x = m_Horizontal * xForce;
        //m_rigid.MovePosition(m_rigid.position + force);
        //m_anim.SetFloat(m_VxState, m_Horizontal);
        //force.x = 0f;
    }

    private void SetCapsuleMaterial(PhysicsMaterial2D mate)
    {
        if (useFZero)
        {
            capsuleColl.sharedMaterial = mate;
        }
    }

    private void CtrlNailiValue()
    {
        if (isDash)
        {
            //减少耐力值
            nailiSlider.value -= nailiReduceRate * Time.deltaTime;
            if (nailiSlider.value <= 0)
            {
                isDash = false;
                nailiSlider.value = 0;
            }
            trailEffect.SetActive(true);
        }
        else
        {
            //回复耐力值
            nailiSlider.value += (nailiReduceRate + 5) * Time.deltaTime;
            if (nailiSlider.value >= nailiMax)
            {
                nailiSlider.value = nailiMax;
            }
            trailEffect.SetActive(false);
        }
    }

    #region 使用AttackInt做的attack
    private void Attack()
    {
        if (!isAttack)
            return;

        //first Attack
        if (m_CurrentAttackCount <= 0 && AttackCountTotal > 0 && m_anim.GetCurrentAnimatorStateInfo(0).IsName("LocoMotion"))
        {
            m_CurrentAttackCount++;
            attackInt = m_CurrentAttackCount;
        }
        //more Attack
        if (m_WaitNextAttack != null && m_CurrentAttackCount < AttackCountTotal)
        {
            m_CurrentAttackCount++;
            attackInt = m_CurrentAttackCount;
            StopCoroutine(m_WaitNextAttack);
            m_WaitNextAttack = null;
        }
        m_anim.SetInteger(m_AttatckState, attackInt);
    }

    public void WaitForNextAttackInput()
    {
        m_WaitNextAttack = WaitNextAttack();
        StartCoroutine(m_WaitNextAttack);
    }

    private IEnumerator WaitNextAttack()
    {
        yield return new WaitForSeconds(AttackWaitTime);
        attackInt = 0;
        m_anim.SetInteger(m_AttatckState, attackInt);
        m_CurrentAttackCount = 0;
        m_WaitNextAttack = null;
    }

    public void MoveWithAttack()
    {
        if (m_CurrentAttackCount == 1)
        {
            Vector2 position = m_rigid.position + (Vector2)Vector3.Scale(dir, new Vector3(Attack1_Movement, 0, 0));
            m_rigid.MovePosition(position);
        }
    }
    #endregion

    #region 使用Trigger做的attack
    private void AttackByTrigger()
    {
        if (isAttack)
        {
            m_anim.SetTrigger(m_AttatckTriggerState);
        }
    }
    #endregion

    private void TriggerHearyFall()
    {
        m_anim.SetTrigger(m_HearyFallTriggerState);
    }

    #region 碰撞的回调检测是否接触地面(弃用)
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    switch (collision.gameObject.tag)
    //    {
    //        case "Ground":
    //            isGround = true;
    //            isJump = false;
    //            ResetJumpForceCount();
    //            break;
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    switch (collision.gameObject.tag)
    //    {
    //        case "Ground":
    //            Debug.Log("Exit from " + collision.gameObject.tag);
    //            isGround = false;
    //            break;
    //    }
    //}
    #endregion

    #region 获取按键的输入
    private void GetAttackInput()
    {
        //原来的代码
        //if (Input.GetKeyDown(KeyCode.J) & isGround)
        //{
        //    isAttack = true;
        //}
        //if (Input.GetKeyUp(KeyCode.J))
        //{
        //    isAttack = false;
        //}

        ///测试代码
        isAttack = Input.GetKeyDown(KeyCode.J) ? true : false;
    }

    private void GetSitInput()
    {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            isSit = true;
        }
        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            isSit = false;
        }
    }

    private void GetDashInput()
    {
        //可以采用达到一定耐力值以上才能dash的方案
        if (Input.GetKeyDown(KeyCode.L) & isGround)
        {
            isDash = true;
        }
        if (Input.GetKeyUp(KeyCode.L))
        {
            isDash = false;
        }
    }

    private bool GetJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) & isGround)
        {
            isJump = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJump = false;
        }
        m_anim.SetBool(m_IsJumpState, isJump);
        return isJump;
    }
    #endregion

    #region SMB调用的接口
    private void InitReference()
    {
        m_anim = GetComponent<Animator>();
        try
        {
            //anim.GetBehaviour<LocoMotionSMB>().roleContr = this;
            //anim.GetBehaviour<JumpDownSMB>().roleContr = this;
            //anim.GetBehaviour<JumpUpSMB>().roleContr = this;

            var smbs = m_anim.GetBehaviours<stickManBase>();
            for (int i = 0; i < smbs.Length; i++)
            {
                smbs[i].monoBehaviour = this;
            }
        }
        catch (System.Exception e)
        {
            throw e;
        }

        //damager = GetComponent<Damager>();
        m_rigid = GetComponent<Rigidbody2D>();
        m_characterController2D = GetComponent<CharacterController2D>();
    }

    public void Jump()
    {
        if (m_CurrentAttackCount > 0)
            return;

        if (isJump && JumpForceCount > 0)
        {
            temp.x = m_rigid.velocity.x;
            temp.y = ySpeed;
            m_rigid.velocity = temp;
            JumpForceCount -= 0.1f;
        }

        temp = Vector2.zero;
    }

    private void ResetJumpForceCount()
    {
        JumpForceCount = JumpForceCountTotle;
    }
    
    public void CheckFace()
    {
        m_Horizontal = Input.GetAxisRaw("Horizontal");

        dir = transform.localScale;
        if (m_Horizontal > 0)
        {
            dir.x = 1;
        }
        else if (m_Horizontal < 0)
        {
            dir.x = -1;
        }
        transform.localScale = dir;
    }
    
    public void GroundMovement()
    {
        if (m_CurrentAttackCount > 0)
            return;

        // 移动 当蹲下时不能移动       // 或者可以采用匍匐前进的方式，即蹲下时速度减半的效果
        if (isSit)
            return;

        if (isDash)
        {
            force.x = m_Horizontal * xDashForce;
            m_Vx_Max = vxDashMax;
        }
        else
        {
            force.x = m_Horizontal * xForce;
            m_Vx_Max = vxMax;
        }
        force.y = m_rigid.velocity.y;
    }

    public void CheckGround()
    {
        if (m_characterController2D.IsGrounded)
        {
            if (!isGround)
            {
                CancelJumpSignal();
                ResetJumpForceCount();
            }
            isGround = true;
            SetCapsuleMaterial(null);
        }
        else
        {
            isGround = false;
            SetCapsuleMaterial(FZero);
        }
        m_anim.SetBool(m_IsGroundState, isGround);
    }

    public void PlusCurrentAttackCount()
    {
        m_CurrentAttackCount++;
    }

    public void ResetCurrentAttackCount()
    {
        m_CurrentAttackCount = 0;
    } 

    public void CheckCeil()
    {
        //碰到天花板后停止跳跃
        if (m_characterController2D.IsCeilinged)
        {
            CancelJumpSignal();
            //temp.x = m_rigid.velocity.x;
            //temp.y = 0;
            //m_rigid.velocity = temp;
            ResetJumpForceCount();
        }
    }

    public bool CheckJump()
    {
        return GetJumpInput();
    }

    public void CancelJumpSignal()
    {
        isJump = false;
        m_rigid.velocity = new Vector2(m_rigid.velocity.x, 0);
        m_anim.SetBool(m_IsJumpState, isJump);
    }

    public void CheckHearyFall()
    {
        if (-hearyFall.hearyFallSpeedLine > m_rigid.velocity.y && hearyFall.CheckGround())
        {
            TriggerHearyFall();
        }
    }

    public void ShakeCamera()
    {
        CameraShaker.Shake(0.15f, 0.3f);
    }

    public void LockSpeed()
    {
        m_rigid.velocity = Vector2.zero;
    }
    #endregion
}