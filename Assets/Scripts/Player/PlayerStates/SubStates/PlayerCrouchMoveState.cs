using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchMoveState : PlayerGroundedState
{
    public PlayerCrouchMoveState(PlayerMoveController player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : base(player, playerStateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetColliderHeight(playerData.crouchColliderHeight);
    }

    public override void Exit()
    {
        base.Exit();
        player.SetColliderHeight(playerData.standColliderHeight);
    }

    public override void LogicUpdate()
    {
        if(!hasExited)
        {
            player.FlipIfNeed(player.InputHandler.NormInputX);
            player.SetVelocityX(playerData.croughMovementVelocity*player.InputHandler.NormInputX);
            if(player.InputHandler.NormInputX==0)
            {
                stateMachine.ChangeState(player.CrouchIdleState);
            }
            else if(player.InputHandler.NormInputY!=-1 && !player.CheckCeiling())
            {
                stateMachine.ChangeState(player.MoveState);
            }
            else if(player.InputHandler.JumpInput && player.JumpState.CanJump())
            {
                if(!player.CheckCeiling())
                {
                    player.InputHandler.UseJumpInput();
                    stateMachine.ChangeState(player.JumpState);
                }
            }
            else if(player.InputHandler.DashInput && player.DashState.CheckIfCanDash())
            {
                if(!player.CheckCeiling())
                {
                    stateMachine.ChangeState(player.DashState);
                    player.InputHandler.UseDashInput();
                }
            }
            else
                base.LogicUpdate();
        }
    }
}
