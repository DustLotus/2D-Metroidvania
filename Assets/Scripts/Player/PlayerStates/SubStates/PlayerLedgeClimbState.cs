using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeClimbState : PlayerState
{

    private Vector2 detectedPos;
    private Vector2 cornerPos;
    private Vector2 startPos;
    private Vector2 stopPos;
    private Vector2 workspace;

    private bool isHanging;
    private bool isClimbing;
    private bool JumpInput;
    private bool isTouchingCeiling;
    private bool aimInput;

    private int xInput;
    private int yInput;

    public PlayerLedgeClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        player.Anim.SetBool("climbLedge", false);
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        isHanging = true;
    }

    public override void Enter()
    {
        base.Enter();

        core.Movement.SetVelocityZero();
        player.transform.position = detectedPos;
        cornerPos = DetermineCornerPosition();

        startPos.Set(cornerPos.x - (core.Movement.FacingDirection * playerData.startOffset.x), cornerPos.y - playerData.startOffset.y);
        stopPos.Set(cornerPos.x + (core.Movement.FacingDirection * playerData.stopOffset.x), cornerPos.y + playerData.stopOffset.y);

        player.transform.position = startPos;
    }

    public override void Exit()
    {
        base.Exit();

        isHanging = false;

        if (isClimbing)
        {
            player.transform.position = stopPos;
            isClimbing = false;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        aimInput = player.InputHandler.AimInput;

        if (isAnimationFinished)
        {
            if (isTouchingCeiling)
            {
                stateMachine.ChangeState(player.CrouchIdleState);
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
        else
        {
            xInput = player.InputHandler.NormInputX;
            yInput = player.InputHandler.NormInputY;
            JumpInput = player.InputHandler.JumpInput;
            

            core.Movement.SetVelocityZero();
            player.transform.position = startPos;

            if ((xInput == core.Movement.FacingDirection || JumpInput) && isHanging && !isClimbing)
            {
                CheckForSpace();
                isClimbing = true;
                player.Anim.SetBool("climbLedge", true);
            }
            else if ((yInput == -1) && isHanging && !isClimbing)
            {
                stateMachine.ChangeState(player.InAirState);
            }
            else if ((yInput == -1) && aimInput && isHanging && !isClimbing)
            {
                stateMachine.ChangeState(player.InAirAimState);
            }
        }
    }

    public void SetDetectedPosition(Vector2 pos) => detectedPos = pos;

    private void CheckForSpace()
    {
        isTouchingCeiling = Physics2D.Raycast(cornerPos + (Vector2.up * 0.015f) + (Vector2.right * core.Movement.FacingDirection * 0.015f), Vector2.up, playerData.standColliderHeight, core.CollisionSenses.WhatIsGround);
        player.Anim.SetBool("isTouchingCeiling", isTouchingCeiling);
    }

    private Vector2 DetermineCornerPosition()
    {
        RaycastHit2D xHit = Physics2D.Raycast(core.CollisionSenses.WallCheck.position, Vector2.right * core.Movement.FacingDirection, core.CollisionSenses.WallCheckDistance, core.CollisionSenses.WhatIsGround);
        float xDist = xHit.distance;
        workspace.Set((xDist + 0.015f) * core.Movement.FacingDirection, 0f);
        RaycastHit2D yHit = Physics2D.Raycast(core.CollisionSenses.LedgeCheck.position + (Vector3)(workspace), Vector2.down, core.CollisionSenses.LedgeCheck.position.y - core.CollisionSenses.WallCheck.position.y + 0.015f, core.CollisionSenses.WhatIsGround);
        float yDist = yHit.distance;

        workspace.Set(core.CollisionSenses.WallCheck.position.x + (xDist * core.Movement.FacingDirection), core.CollisionSenses.LedgeCheck.position.y - yDist);
        return workspace;
    }
}
