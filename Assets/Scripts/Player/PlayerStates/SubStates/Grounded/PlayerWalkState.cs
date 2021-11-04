using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerGroundedState
{
    private bool WalkInput;

    public PlayerWalkState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
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

        core.Movement.SetVelocityX(playerData.walkMovementVelocity * core.Movement.FacingDirection);
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
            else if (!WalkInput)
            {
                stateMachine.ChangeState(player.MoveState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

