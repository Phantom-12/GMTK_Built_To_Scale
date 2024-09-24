using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerGroundedState
{
    public PlayerLandState(PlayerMoveController player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : base(player, playerStateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(!hasExited)
        {
            if(player.InputHandler.NormInputX!=0)
            {
                stateMachine.ChangeState(player.MoveState);
            }
            // else if(player.InputHandler.NormInputY==-1)
            // {
            //     stateMachine.ChangeState(player.CrouchIdleState);
            // }
            // else if(isAnimationFinished)
            // {
            //     stateMachine.ChangeState(player.IdleState);
            // }
            else
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
    }
}
