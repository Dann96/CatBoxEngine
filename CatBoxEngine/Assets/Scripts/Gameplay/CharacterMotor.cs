using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
//[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class CharacterMotor : MonoBehaviour
{
    public static CharacterMotor s_player;

    public string characterTag = "Gregg";
    public float movingTurningSpeed = 360.0f;
    public float stationaryTurnSpeed = 180.0f;
    public float gravityMultiplier = 2.0f;
    public float moveSpeedMultiplier = 1.0f;
    public float groundCheckDistance = 0.3f;
    public float animationSpeedMultiplier = 1.0f;

    private float m_OriginalGroundCheckDistance;

    private Rigidbody m_Rigidbody;
    private CapsuleCollider m_CapsuleCollider;
    private Animator m_Animator;
    private NavMeshAgent m_Agent;

    private PhysicMaterial maxFrictionPhysics, frictionPhysics, slipperyPhysics;

    private bool m_Grounded;
    private float m_ForwardAmount;
    private float m_TurnAmount;
    private Vector3 m_GroundNormal;

    private Vector3 m_Input;

    public static CharacterMotor Player
    {
        get { return s_player; }
    }

    public bool IsPlayer
    {
        get { return (this == s_player); }
    }

    public bool pathPending
    {
        get { return m_Agent.pathPending; }
    }

    public float remainingDistance
    {
        get { return m_Agent.remainingDistance; }
    }


    public Vector3 AgentVelocity
    {
        get{ return m_Agent.desiredVelocity; }
    }

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_CapsuleCollider = GetComponent<CapsuleCollider>();
        m_Animator = GetComponent<Animator>();
        m_Agent = GetComponent<NavMeshAgent>();
        m_Agent.updatePosition = false;
        m_Agent.updateRotation = false;

        m_OriginalGroundCheckDistance = groundCheckDistance;

        frictionPhysics = new PhysicMaterial("frictionPhysics");
        frictionPhysics.staticFriction = 0.25f;
        frictionPhysics.dynamicFriction = 0.25f;
        frictionPhysics.frictionCombine = PhysicMaterialCombine.Multiply;

        maxFrictionPhysics = new PhysicMaterial("maxFrictionPhysics");
        maxFrictionPhysics.staticFriction = 1.0f;
        maxFrictionPhysics.dynamicFriction = 1.0f;
        maxFrictionPhysics.frictionCombine = PhysicMaterialCombine.Maximum;

        slipperyPhysics = new PhysicMaterial("slipperyPhysics");
        slipperyPhysics.staticFriction = 0.0f;
        slipperyPhysics.dynamicFriction = 0.0f;
        slipperyPhysics.frictionCombine = PhysicMaterialCombine.Minimum;
    }

    void FixedUpdate()
    {
        if (!m_Animator)
        {
            if (m_Grounded && Time.deltaTime > 0)
            {
                Vector3 v = ((transform.forward * m_ForwardAmount) * moveSpeedMultiplier) / Time.deltaTime;

                v.y = m_Rigidbody.velocity.y;
                m_Rigidbody.velocity = v;
            }
        }


        Move(m_Input);

        m_Agent.nextPosition = transform.position;
    }
    public void Move(Vector3 move)
    {
        if (move.magnitude > 1f)
            move.Normalize();
        move = transform.InverseTransformDirection(move);
        m_TurnAmount = Mathf.Atan2(move.x, move.z);
        m_ForwardAmount = move.z;
        ApplyTurnRotation();

        CheckGroundStatus();

        //HandleBasicMovement(forward * input.y + right * input.x);
        //if (m_Grounded)
        //    HandleGroundedMovement(false);
        //else HandleAirborneMovement();

        if (m_Grounded && Mathf.Approximately(move.normalized.magnitude, 0))
            m_CapsuleCollider.material = maxFrictionPhysics;
        else if (m_Grounded && !Mathf.Approximately(move.normalized.magnitude, 0))
            m_CapsuleCollider.material = frictionPhysics;
        else
            m_CapsuleCollider.material = slipperyPhysics;

        UpdateAnimator(move);
    }

    void HandleAirborneMovement()
    {
        Vector3 extraGravityForce = (Physics.gravity * gravityMultiplier) - Physics.gravity;
        m_Rigidbody.AddForce(extraGravityForce);
        groundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OriginalGroundCheckDistance : 0.01f;

        Debug.DrawRay(transform.position + transform.up, transform.forward, Color.yellow);
    }

    void HandleGroundedMovement(bool jump)
    {
        /*if (jump && m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        {
            m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, jumpPower, m_Rigidbody.velocity.z);
            m_Grounded = false;
            m_Animator.applyRootMotion = false;
            groundCheckDistance = 0.1f;
        }*/
    }

    void UpdateAnimator(Vector3 move)
    {
        if (m_Animator)
        {
            m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
            m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
            m_Animator.SetBool("Grounded", m_Grounded);

            if (!m_Grounded)
                m_Animator.SetFloat("Jump", m_Rigidbody.velocity.y);

            m_Animator.speed = (m_Grounded && move.magnitude > 0) ? animationSpeedMultiplier : 1.0f;
        }
    }

    void ApplyTurnRotation()
    {
        float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurningSpeed, m_ForwardAmount);
        transform.Rotate(Vector3.up * m_TurnAmount * turnSpeed * Time.deltaTime);
    }

    public void OnAnimatorMove()
    {
        if (m_Animator)
        {
            if (m_Grounded && Time.deltaTime > 0)
            {
                Vector3 v = (m_Animator.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;

                v.y = m_Rigidbody.velocity.y;
                m_Rigidbody.velocity = v;
            }
        }
    }

    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
        Debug.DrawLine(transform.position + (transform.up * 0.1f), transform.position - transform.up * groundCheckDistance, Color.cyan);
        if (Physics.Raycast(transform.position + (transform.up * 0.1f), -transform.up, out hitInfo, groundCheckDistance))
        {

            m_GroundNormal = hitInfo.normal;
            m_Grounded = true;
            //m_Animator.applyRootMotion = true;
        }
        else
        {
            m_Grounded = false;
            m_GroundNormal = Vector3.up;
            //m_Animator.applyRootMotion = false;
        }
    }

    public void RelayInput(Vector3 input)
    {
        m_Input = input;
    }

    public void SetAsPlayer()
    {
        s_player = this;
        Debug.Log("New Player Motor: " + this.characterTag);
        CameraController.instance.SetTarget(this.transform);
    }

    public void SetAgentDestination(Vector3 pos)
    {
        m_Agent.SetDestination(pos);
    }
}
