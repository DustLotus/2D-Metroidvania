using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    private bool WalkInput;

    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        
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
        
        core.Movement.SetVelocityX(playerData.movementVelocity * xInput);
        core.Movement.CheckIfShouldFlip(xInput);

        if (!isExitingState)
        {
            WalkInput = player.InputHandler.WalkInput;

            if (xInput == 0)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else if (yInput == -1)
            {
                stateMachine.ChangeState(player.CrouchMoveState);
            }
            else if (WalkInput)
            {
                stateMachine.ChangeState(player.WalkState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
