using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallGrabState : PlayerTouchingWallState
{
    private Vector2 targetHoldPosition;

    private int yInput;
    private bool grabInput;

    public PlayerWallGrabState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : base(player, playerStateMachine, playerData, animBoolName)
    {
    }

    public override void Dochecks()
    {
        base.Dochecks();
    }

    public override void Enter()
    {
        base.Enter();
        targetHoldPosition=player.transform.position;
        HoldPosition();
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
            yInput=player.InputHandler.NormInputY;
            grabInput=player.InputHandler.GrabInput;
            if(yInput>0)
            {
                stateMachine.ChangeState(player.WallClimbState);
            }
            else if(yInput<0 || !grabInput)
            {
                stateMachine.ChangeState(player.WallSlideState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        player.SetVelocityX(0);
        player.SetVelocityY(0);
        HoldPosition();
    }

    private void HoldPosition()
    {
        player.transform.position=targetHoldPosition;
    }
}
