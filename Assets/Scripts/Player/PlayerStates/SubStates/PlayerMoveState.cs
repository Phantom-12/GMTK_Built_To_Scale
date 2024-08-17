using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(PlayerMoveController player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : base(player, playerStateMachine, playerData, animBoolName)
    {
    }

    public override void Dochecks()
    {
        base.Dochecks();
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
        if(!hasExited)
        {
            player.FlipIfNeed(player.InputHandler.NormInputX);
            if(player.InputHandler.NormInputX==0)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            // else if(player.InputHandler.NormInputY==-1)
            // {
            //     stateMachine.ChangeState(player.CrouchMoveState);
            // }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        player.SetVelocityX(playerData.movementVelocity*player.InputHandler.NormInputX);
    }
}
