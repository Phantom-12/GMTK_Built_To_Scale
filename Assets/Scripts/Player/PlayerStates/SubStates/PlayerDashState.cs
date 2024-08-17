using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerAbilityState
{
    public bool CanDash{get;private set;}

    private float lastDashTime;
    private Vector2 lastAfterImagePosition;

    private bool isHolding;
    private Vector2 dashDirection;

    public PlayerDashState(PlayerMoveController player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : base(player, playerStateMachine, playerData, animBoolName)
    {
    }

    public override void Dochecks()
    {
        base.Dochecks();
    }

    public override void Enter()
    {
        base.Enter();

        CanDash=false;
        isHolding=true;
        dashDirection=Vector2.right*player.FacingDirection;

        Time.timeScale=playerData.dashTimeScale;
        Time.fixedDeltaTime*=playerData.dashTimeScale;

        player.DashDirectionIndicator.gameObject.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();
        if(player.Rb.velocity.y>0)
            player.SetVelocityY(player.Rb.velocity.y*playerData.dashEndYMutiplier);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(!hasExited)
        {
            if(isHolding)
            {
                if(player.InputHandler.DashDirectionInput!=Vector2.zero)
                {
                    dashDirection=player.InputHandler.DashDirectionInput;
                    dashDirection.Normalize();
                }

                player.DashDirectionIndicator.rotation=Quaternion.Euler(0,0,Vector2.SignedAngle(Vector2.right,dashDirection)-45f);

                if(player.InputHandler.DashInputStop || (Time.time-startTime)/playerData.dashTimeScale>playerData.maxDashHoldTime)
                {
                    player.InputHandler.UseDashInputStop();
                    isHolding=false;
                    Time.timeScale=1f;
                    Time.fixedDeltaTime/=playerData.dashTimeScale;
                    startTime=Time.time;
                    player.FlipIfNeed(Mathf.RoundToInt(dashDirection.x));
                    player.Rb.drag=playerData.drag;
                    player.SetVelocity(playerData.dashVeclocity,dashDirection);
                    player.DashDirectionIndicator.gameObject.SetActive(false);
                }
            }
            else
            {
                if(Vector2.Distance(lastAfterImagePosition,player.transform.position)>=playerData.distanceBetweenAfterImages)
                {
                    PlayerAfterImagePool.Pool.Get();
                    lastAfterImagePosition=player.transform.position;
                }
                if(Time.time>startTime+playerData.dashTime)
                {
                    player.Rb.drag=0;
                    isAbilityDone=true;
                    lastDashTime=Time.time;
                }
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        player.Anim.SetFloat("xVelocity",Mathf.Abs(player.Rb.velocity.x));
        player.Anim.SetFloat("yVelocity",player.Rb.velocity.y);
        if(!isHolding)
            player.SetVelocity(playerData.dashVeclocity,dashDirection);
    }

    public bool CheckIfCanDash()
    {
        return CanDash && Time.time>lastDashTime+playerData.dashCooldown;
    }

    public void ResetCanDash()
    {
        CanDash=true;
    }
}
