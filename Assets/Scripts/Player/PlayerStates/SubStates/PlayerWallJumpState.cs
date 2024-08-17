using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerAbilityState
{
    private int wallJumpDirection;

    public PlayerWallJumpState(PlayerMoveController player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : base(player, playerStateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.JumpState.ResetJumpTimesLeft();
        wallJumpDirection=player.CheckIfTouchingWall()?-player.FacingDirection:player.FacingDirection;
        player.SetVelocity(playerData.wallJumpVelocity,wallJumpDirection>0?playerData.wallJumpAngle:Vector2.Reflect(playerData.wallJumpAngle,Vector2.right));
        player.FlipIfNeed(wallJumpDirection);
        player.JumpState.DecreaseJumpTimesLeft();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(!hasExited)
        {
            player.Anim.SetFloat("xVelocity",Mathf.Abs(player.Rb.velocity.x));
            player.Anim.SetFloat("yVelocity",player.Rb.velocity.y);
            if(Time.time>startTime+playerData.wallJumpTime)
            {
                isAbilityDone=true;
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
