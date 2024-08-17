using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchingWallState : PlayerState
{
    protected bool isGrounded;
    protected bool isTouchingWall,isTouchingLegde;

    public PlayerTouchingWallState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : base(player, playerStateMachine, playerData, animBoolName)
    {
    }

    public override void Dochecks()
    {
        base.Dochecks();
        isGrounded=player.CheckGrounded();
        isTouchingWall=player.CheckIfTouchingWall();
        isTouchingLegde=player.CheckIfTouchingLedge();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        if(isGrounded && !player.InputHandler.GrabInput)
        {
            stateMachine.ChangeState(player.IdleState);
        }
        else if(isTouchingWall && player.InputHandler.JumpInput)
        {
            player.InputHandler.UseJumpInput();
            stateMachine.ChangeState(player.WallJumpState);
        }
        else if(!isTouchingWall || (player.InputHandler.NormInputX!=player.FacingDirection && !player.InputHandler.GrabInput))
        {
            stateMachine.ChangeState(player.InAirState);
        }
        else if(!isTouchingLegde && isTouchingWall)
        {
            player.LedgeClimbState.SetDetectedPosition(player.transform.position);
            stateMachine.ChangeState(player.LedgeClimbState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
