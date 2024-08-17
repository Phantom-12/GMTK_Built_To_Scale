using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveController : MonoBehaviour
{
    #region State Machine Variables 状态机变量
    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerWallClimbState WallClimbState { get; private set; }
    public PlayerWallGrabState WallGrabState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerLedgeClimbState LedgeClimbState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerCrouchIdleState CrouchIdleState { get; private set; }
    public PlayerCrouchMoveState CrouchMoveState { get; private set; }

    [SerializeField]
    private PlayerData playerData;
    #endregion

    #region Components 组件
    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public BoxCollider2D MovementCollider { get; private set; }
    public Rigidbody2D Rb { get; private set; }
    public Transform DashDirectionIndicator { get; private set; }
    public PlayerInput playerInput;
    #endregion

    #region Check Transforms 检测点
    [SerializeField]
    private Transform groundCheckPoint;
    [SerializeField]
    private Transform wallCheckPoint;
    [SerializeField]
    private Transform ledgeCheckPoint;
    [SerializeField]
    private Transform CeilingCheckPoint;
    #endregion

    #region Other Variables 其他变量
    public int FacingDirection { get; private set; }
    private Vector2 workspace;
    #endregion

    #region Unity Callback Functions unity回调函数 
    private void Awake()
    {
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
        LandState = new PlayerLandState(this, StateMachine, playerData, "land");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
    }

    private void Start()
    {
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        Rb = GetComponent<Rigidbody2D>();
        MovementCollider = GetComponent<BoxCollider2D>();
        DashDirectionIndicator = transform.Find("DashDirectionIndicator");

        FacingDirection = 1;

        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }
    #endregion

    #region Set Funtions 设置函数
    public void SetVelocityX(float velocity)
    {
        workspace.Set(velocity, Rb.velocity.y);
        Rb.velocity = workspace;
    }

    public void SetVelocityY(float velocity)
    {
        workspace.Set(Rb.velocity.x, velocity);
        Rb.velocity = workspace;
    }

    public void SetVelocity(float velocity, Vector2 direction)
    {
        direction.Normalize();
        workspace.Set(direction.x * velocity, direction.y * velocity);
        Rb.velocity = workspace;
    }

    public void SetColliderHeight(float height)
    {
        Vector2 center = MovementCollider.offset;
        workspace.Set(MovementCollider.size.x, height);
        center.y += (height - MovementCollider.size.y) / 2;
        MovementCollider.size = workspace;
        MovementCollider.offset = center;
    }
    #endregion

    #region Check Funtions 检测函数
    public void FlipIfNeed(int xInput)
    {
        if (xInput != 0 && xInput != FacingDirection)
            Flip();
    }

    public Vector2 DetermineCornerPosition()
    {
        RaycastHit2D xHit = Physics2D.Raycast(wallCheckPoint.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
        RaycastHit2D yHit = Physics2D.Raycast(ledgeCheckPoint.position + new Vector3(FacingDirection * playerData.wallCheckDistance, 0, 0), Vector2.down, ledgeCheckPoint.position.y - wallCheckPoint.position.y, playerData.whatIsGround);
        return new Vector2(wallCheckPoint.position.x + xHit.distance * FacingDirection, ledgeCheckPoint.position.y - yHit.distance);
    }

    public bool CheckGrounded()
    {
        return Physics2D.OverlapCircle(groundCheckPoint.position, playerData.groundCheckRadius, playerData.whatIsGround);
    }

    public bool CheckIfTouchingWall()
    {
        return Physics2D.Raycast(wallCheckPoint.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }

    public bool CheckIfTouchingWallBack()
    {
        return Physics2D.Raycast(wallCheckPoint.position, Vector2.left * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }

    public bool CheckIfTouchingLedge()
    {
        return Physics2D.Raycast(ledgeCheckPoint.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }

    public bool CheckCeiling()
    {
        return Physics2D.OverlapCircle(CeilingCheckPoint.position, playerData.groundCheckRadius, playerData.whatIsGround);
    }
    #endregion

    #region Other Funtions 其他函数
    private void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void AnimationTrigger()
    {
        StateMachine.CurrentState.AnimationTrigger();
    }

    private void AnimationFinishedTrigger()
    {
        StateMachine.CurrentState.AnimationFinishedTrigger();
    }

    public void OnDrawGizmos()
    {
        // 地面检测范围显示
        // Gizmos.DrawSphere(groundCheckPoint.position, playerData.groundCheckRadius);
    }

    public void SetEnable(bool enable)
    {
        playerInput.enabled = enable;
    }
    #endregion
}
