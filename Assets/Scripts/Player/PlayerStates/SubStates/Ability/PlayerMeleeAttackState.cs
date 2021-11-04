using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttackState : PlayerAbilityState
{
    private Weapon weapon;

    private int mouseSide;

    private float velocityToSet;

    private bool isGrounded;
    private bool setVelocity;
    private bool shouldCheckFlip;

    public PlayerMeleeAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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

        setVelocity = false;

        weapon.EnterWeapon();
    }

    public override void Exit()
    {
        base.Exit();

        weapon.ExitWeapon();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        mouseSide = player.InputHandler.MouseSide;

        if (shouldCheckFlip)
        {
            core.Movement.CheckIfShouldFlip(mouseSide);
        }

        if (setVelocity)
        {
            core.Movement.SetVelocityX(velocityToSet * core.Movement.FacingDirection);
        }
    }

    public void SetWeapon(Weapon weapon)
    {
        this.weapon = weapon;
        this.weapon.InitializeWeapon(this);
    }

    public void SetPlayerVelocity(float velocity)
    {
        if (isGrounded)
        {
            core.Movement.SetVelocityX(velocity * core.Movement.FacingDirection);

            velocityToSet = velocity;
            setVelocity = true;
        }
    }

    public void SetFlipCheck(bool value)
    {
        shouldCheckFlip = value;
    }


    #region Animation Triggers

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAbilityDone = true;
    }

    #endregion

}
