using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    protected int xInput;
    protected int yInput;
    private bool jumpInput;
    private bool dashInput;
    private bool aimInput;

    protected bool isTouchingCeiling;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingLedge;

    private bool isAiming;

    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = core.CollisionSenses.Ground;
        isTouchingWall = core.CollisionSenses.Wall;
        isTouchingCeiling = core.CollisionSenses.Ceiling;
        isTouchingLedge = core.CollisionSenses.Ledge;
        isAiming = player.InputHandler.aiming;
    }

    public override void Enter()
    {
        base.Enter();

        player.JumpState.ResetAmountOfJumpsLeft();
        player.DashState.ResetCanDash();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.NormInputX;
        yInput = player.InputHandler.NormInputY;
        jumpInput = player.InputHandler.JumpInput;
        dashInput = player.InputHandler.DashInput;
        aimInput = player.InputHandler.AimInput;

        if (player.InputHandler.AttackInputs[(int)CombatInputs.primary] && !isAiming && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.PrimaryMeleeAttackState);
        }
        else if (player.InputHandler.AttackInputs[(int)CombatInputs.secondary] && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.RangedAttackState);
        }
        else if (aimInput && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.IdleAimState);
        }
        else if (jumpInput && player.JumpState.CanJump())
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else if (dashInput && player.DashState.CheckIfCanDash())
        {
            stateMachine.ChangeState(player.DashState);
        }
        else if (!isGrounded && !aimInput)
        {
            player.InAirState.StartCoyoteTime();
            stateMachine.ChangeState(player.InAirState);
        }
        else if (!isGrounded && aimInput)
        {
            player.InAirAimState.StartCoyoteTime();
            stateMachine.ChangeState(player.InAirAimState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

}
