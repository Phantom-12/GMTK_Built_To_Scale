using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(PlayerMoveController player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : base(player, playerStateMachine, playerData, animBoolName)
    {
    }

    public override void Dochecks()
    {
        base.Dochecks();
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocityX(0);
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
            if(player.InputHandler.NormInputX!=0f)
            {
                stateMachine.ChangeState(player.MoveState);
            }
            // else if(player.InputHandler.NormInputY==-1)
            // {
            //     stateMachine.ChangeState(player.CrouchIdleState);
            // }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if(!hasExited)
        {
            player.SetVelocity(0,Vector2.zero);
        }
    }
}
