using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    private bool isGrounded;
    private bool isTouchingWall;

    public PlayerGroundedState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : base(player, playerStateMachine, playerData, animBoolName)
    {
    }

    public override void Dochecks()
    {
        base.Dochecks();
        isGrounded=player.CheckGrounded();
        isTouchingWall=player.CheckIfTouchingWall();
    }

    public override void Enter()
    {
        base.Enter();
        player.JumpState.ResetJumpTimesLeft();
        // player.DashState.ResetCanDash();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(!hasExited)
        {
            if(player.InputHandler.JumpInput && player.JumpState.CanJump())
            {
                player.InputHandler.UseJumpInput();
                stateMachine.ChangeState(player.JumpState);
            }
            else if(!isGrounded)
            {
                player.InAirState.StartCoyoteTime();
                stateMachine.ChangeState(player.InAirState);
            }
            // else if(player.InputHandler.DashInput && player.DashState.CheckIfCanDash())
            // {
            //     stateMachine.ChangeState(player.DashState);
            //     player.InputHandler.UseDashInput();
            // }
            // else if(isTouchingWall && player.InputHandler.GrabInput)
            // {
            //     stateMachine.ChangeState(player.WallGrabState);
            // }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
