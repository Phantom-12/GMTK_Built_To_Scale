using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    private int jumpTimesLeft;

    public PlayerJumpState(PlayerMoveController player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : base(player, playerStateMachine, playerData, animBoolName)
    {
        jumpTimesLeft=playerData.maxJumpTimes;
    }

    public override void Enter()
    {
        SoundManager.Instance.SceneEffectPlayStr("15");
        player.SetVelocityY(playerData.jumpVelocity);
        player.Anim.SetFloat("yVelocity",player.Rb.velocity.y);
        base.Enter();
        DecreaseJumpTimesLeft();
        player.InAirState.SetIsJumping();
        isAbilityDone=true;
    }

    public bool CanJump()
    {
        return jumpTimesLeft>0;
    }

    public void ResetJumpTimesLeft()
    {
        jumpTimesLeft=playerData.maxJumpTimes;
    }

    public void DecreaseJumpTimesLeft()
    {
        jumpTimesLeft--;
    }
}
