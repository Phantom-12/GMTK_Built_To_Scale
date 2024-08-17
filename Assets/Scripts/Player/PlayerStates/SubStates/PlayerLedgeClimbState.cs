using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeClimbState : PlayerState
{
    private Vector2 detectedPosition;
    private Vector2 cornerPosition;
    private Vector2 startPosition;
    private Vector2 stopPosition;

    private bool isHanging,isClimbing;
    private bool isTouchingCeiling;

    public PlayerLedgeClimbState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : base(player, playerStateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.transform.position=detectedPosition;
        player.SetVelocity(0,Vector2.zero);
        cornerPosition=player.DetermineCornerPosition();

        startPosition.Set(cornerPosition.x-player.FacingDirection*playerData.startOffest.x,cornerPosition.y-playerData.startOffest.y);
        stopPosition.Set(cornerPosition.x+player.FacingDirection*playerData.stopOffest.x,cornerPosition.y+playerData.stopOffest.y);

        player.transform.position=startPosition;
    }

    public override void Exit()
    {
        base.Exit();

        isHanging=false;

        if(isClimbing)
        {
            isClimbing=false;
            player.transform.position=stopPosition;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isAnimationFinished)
        {
            if(isTouchingCeiling)
            {
                stateMachine.ChangeState(player.CrouchIdleState);
            }
            else
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
        else if((player.InputHandler.NormInputX==player.FacingDirection || player.InputHandler.JumpInput) && isHanging && !isClimbing)
        {
            CheckCeiling();
            if(player.InputHandler.JumpInput)
                player.InputHandler.UseJumpInput();
            isClimbing=true;
            if(isTouchingCeiling)
                player.Anim.SetBool("ledgeClimb",true);
            else
                player.Anim.SetBool("ledgeClimbCrouch",true);
        }
        else if(player.InputHandler.NormInputY==-1 && isHanging && !isClimbing)
        {
            stateMachine.ChangeState(player.InAirState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        player.SetVelocity(0,Vector2.zero);
        player.transform.position=startPosition;
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        isHanging=true;
    }

    public override void AnimationFinishedTrigger()
    {
        base.AnimationFinishedTrigger();

        player.Anim.SetBool("ledgeClimb",false);
        player.Anim.SetBool("ledgeClimbCrouch",false);
    }

    public void SetDetectedPosition(Vector2 pos)
    {
        detectedPosition=pos;
    }

    private void CheckCeiling()
    {
        isTouchingCeiling=Physics2D.Raycast(cornerPosition+(Vector2.up*0.015f)+(Vector2.right*0.015f*player.FacingDirection),Vector2.up,playerData.standColliderHeight,playerData.whatIsGround);
    }
}
