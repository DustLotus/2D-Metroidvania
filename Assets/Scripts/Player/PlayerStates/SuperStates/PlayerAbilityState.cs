using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityState : PlayerState
{
    protected bool isAbilityDone;

    private bool isGrounded;

    private bool AimInput;

    public PlayerAbilityState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = core.CollisionSenses.Ground;
    }

    public override void Enter()
    {
        base.Enter();

        isAbilityDone = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        AimInput = player.InputHandler.AimInput;


        if (isAbilityDone)
        {
            if (!AimInput && isGrounded && core.Movement.CurrentVelocity.y < 0.01f)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else if (AimInput && isGrounded && core.Movement.CurrentVelocity.y < 0.01f)
            {
                stateMachine.ChangeState(player.IdleAimState);
            }
            else if (!AimInput && !isGrounded && core.Movement.CurrentVelocity.y > 0.01f)
            {
                stateMachine.ChangeState(player.InAirState);
            }
            else if (AimInput && !isGrounded && core.Movement.CurrentVelocity.y > 0.01f)
            {
                stateMachine.ChangeState(player.InAirAimState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
