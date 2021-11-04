using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRangedAttackState : PlayerAbilityState
{
    private int mouseSide;
    private bool shouldCheckFlip;

    public PlayerRangedAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("Shooting");
    }


    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        mouseSide = player.InputHandler.MouseSide;

        if (shouldCheckFlip)
        {
            core.Movement.CheckIfShouldFlip(mouseSide);
        }
    }


    public void SetFlipCheck(bool value)
    {
        shouldCheckFlip = value;
    }
}
