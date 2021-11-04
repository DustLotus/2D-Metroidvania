using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerAbilityState
{
    public bool CanDash { get; private set; }

    private float lastDashTime;
    private float startDashTime;

    private bool isGrounded;
    private bool isTouchingCeiling;

    private bool shouldCheckFlip;
    private bool aimInput;

    protected int xInput;

    public PlayerDashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = core.CollisionSenses.Ground;
        isTouchingCeiling = core.CollisionSenses.Ceiling;
    }

    
    public override void Enter()
    {
        base.Enter();

        CanDash = false;
        
        SetFlipCheck(false);

        player.InputHandler.UseDashInput();

        startDashTime = Time.time;

        xInput = player.InputHandler.NormInputX;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {

            if (isGrounded)
            {


                SetFlipCheck(false);

                if (xInput == 0)
                {
                    core.Movement.SetVelocityX(playerData.dashVelocity * core.Movement.FacingDirection);
                }
                else
                {
                    core.Movement.SetVelocityX(playerData.dashVelocity * xInput);
                }
            }

            if (shouldCheckFlip)
            {
                core.Movement.CheckIfShouldFlip(xInput);
            }
        }
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAbilityDone = true;
        SetFlipCheck(true);
        lastDashTime = Time.time;

        aimInput = player.InputHandler.AimInput;

        if (!isExitingState)
        {

            if (isTouchingCeiling)
            {
                stateMachine.ChangeState(player.CrouchIdleState);
            }
            else if (!isGrounded && !aimInput)
            {
                stateMachine.ChangeState(player.InAirState);
            }
            else if (!isGrounded && aimInput)
            {
                stateMachine.ChangeState(player.InAirAimState);
            }
            else if (!isTouchingCeiling && !aimInput)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else if (!isTouchingCeiling && aimInput)
            {
                stateMachine.ChangeState(player.IdleAimState);
            }
        }
    }


    public void SetFlipCheck(bool value)
    {
        shouldCheckFlip = value;
    }

    public bool CheckIfCanDash()
    {
        return CanDash && Time.time >= lastDashTime + playerData.dashCooldown;
    }

    public void ResetCanDash() => CanDash = true;

}
