using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkAimState : PlayerGroundedState
{
    private bool AimInput;

    public PlayerWalkAimState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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

            AimInput = player.InputHandler.AimInput;

            if (xInput == 0)
            {
                stateMachine.ChangeState(player.IdleAimState);
            }
            else if (yInput == -1)
            {
                stateMachine.ChangeState(player.CrouchMoveState);
            }
            else if (!AimInput)
            {
                stateMachine.ChangeState(player.MoveState);
            }
        }
    }
}
