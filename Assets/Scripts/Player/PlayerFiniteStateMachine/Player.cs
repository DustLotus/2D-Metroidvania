using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
public class Player : MonoBehaviour
{
    #region State Variables
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerWalkState WalkState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerLedgeClimbState LedgeClimbState { get; private set; }
    public PlayerCrouchIdleState CrouchIdleState { get; private set; }
    public PlayerCrouchMoveState CrouchMoveState { get; private set; }

    public PlayerMeleeAttackState PrimaryMeleeAttackState { get; private set; }
    public PlayerRangedAttackState RangedAttackState { get; private set; }

    public PlayerIdleAimState IdleAimState { get; private set; }
    public PlayerWalkAimState WalkAimState { get; private set; }
    public PlayerInAirAimState InAirAimState { get; private set; }

    [SerializeField]
    private PlayerData playerData;
    #endregion

    #region Components
    public Core Core { get; private set; }
    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public BoxCollider2D MovementCollider { get; private set; }
    public PlayerInventory Inventory { get; private set; }
    #endregion


    #region Other Variables

    private Vector2 workspace;
    #endregion

    #region Unity Callback Functions
    private void Awake()
    {
        Core = GetComponentInChildren<Core>();

        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
        LandState = new PlayerLandState(this, StateMachine, playerData, "land");
        DashState = new PlayerDashState(this, StateMachine, playerData, "dodgeRoll");
        LedgeClimbState = new PlayerLedgeClimbState(this, StateMachine, playerData, "ledgeClimbState");
        CrouchIdleState = new PlayerCrouchIdleState(this, StateMachine, playerData, "crouchIdle");
        CrouchMoveState = new PlayerCrouchMoveState(this, StateMachine, playerData, "crouchMove");
        WalkState = new PlayerWalkState(this, StateMachine, playerData, "walk");

        PrimaryMeleeAttackState = new PlayerMeleeAttackState(this, StateMachine, playerData, "attack");
        RangedAttackState = new PlayerRangedAttackState(this, StateMachine, playerData, "rangedAttack");

        IdleAimState = new PlayerIdleAimState(this, StateMachine, playerData, "idleAim");
        WalkAimState = new PlayerWalkAimState(this, StateMachine, playerData, "walkAim");
        InAirAimState = new PlayerInAirAimState(this, StateMachine, playerData, "inAirAim");
}

    private void Start()
    {
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponent<Rigidbody2D>();
        MovementCollider = GetComponent<BoxCollider2D>();
        Inventory = GetComponent<PlayerInventory>();

        PrimaryMeleeAttackState.SetWeapon(Inventory.weapons[(int)CombatInputs.primary]);
        //RangedAttackState.SetWeapon(Inventory.weapons[(int)CombatInputs.primary]);

        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        Core.LogicUpdate();
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }
    #endregion



    #region Other Functions
    public void SetColliderHeight(float height)
    {
        Vector2 center = MovementCollider.offset;
        workspace.Set(MovementCollider.size.x, height);

        center.y += (height - MovementCollider.size.y) / 2;

        MovementCollider.size = workspace;
        MovementCollider.offset = center;
    }



    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

    private void AnimtionFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();


    #endregion
}