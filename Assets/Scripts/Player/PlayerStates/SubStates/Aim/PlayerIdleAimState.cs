using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleAimState : PlayerGroundedState
{
    private bool AimInput;


    public PlayerIdleAimState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        core.Movement.SetVelocityX(0f);
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

            if (xInput != 0)
            {
                stateMachine.ChangeState(player.WalkAimState);
            }
            else if (yInput == -1)
            {
                stateMachine.ChangeState(player.CrouchIdleState);
            }
            else if (!AimInput)
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
