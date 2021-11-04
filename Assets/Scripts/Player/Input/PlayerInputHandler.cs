using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput controlInputs;

    public Vector2 RawMovementInput { get; private set; }
    public Vector2 MousePosition { get; private set; }
    public int NormInputX { get; private set; }
    public int NormInputY { get; private set; }
    public bool JumpInput { get; private set; }
    public bool JumpInputStop { get; private set; }
    public bool DashInput { get; private set; }
    public bool DashInputStop { get; private set; }
    public bool WalkInput { get; private set; }

    public bool[] AttackInputs { get; private set; }
    public bool AimInput { get; private set; }
    public int MouseSide { get; private set; }

    public bool aiming = false;


    public Camera mainCam;
    public Rigidbody2D armRB;
    public Vector2 armPosition;

    [SerializeField] private float angleOffSet = -45.0f;
    [SerializeField] private float angleOffSetFaceLeft = -225.0f;

    [SerializeField] private float inputHoldTime = 0.2f;

    private float jumpInputStartTime;

    private float dashInputStartTime;

    private void Awake()
    {
        controlInputs = new PlayerInput();
    }

    private void OnEnable()
    {
        controlInputs.Enable();
        controlInputs.Gameplay.MousePosition.performed += OnMouseInput;
    }

    private void OnDisable()
    {
        controlInputs.Disable();
    }

    private void Start()
    {
        aiming = false;

        int count = Enum.GetValues(typeof(CombatInputs)).Length;
        AttackInputs = new bool[count];
    }

    private void Update()
    {
        CheckJumpInputHoldTime();
        CheckDashInputHoldTime();

        var MousePos = mainCam.ScreenToWorldPoint(MousePosition);
        armPosition = mainCam.WorldToScreenPoint(armRB.position);

    }


    public void OnPrimaryAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if(aiming == false)
            {
                AttackInputs[(int)CombatInputs.primary] = true;
            }
            else
            {
                AttackInputs[(int)CombatInputs.secondary] = true;
            }
        }
        if (context.performed)
        {

        }
        if (context.canceled)
        {
            if (aiming == false)
            {
                AttackInputs[(int)CombatInputs.primary] = false;
            }
            else
            {
                AttackInputs[(int)CombatInputs.secondary] = false;
            }
        }
    }

    public void OnAimInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (aiming == false)
            {
                AimInput = true;
                aiming = true;
            }
            else
            {
                AimInput = false;
                aiming = false;
            }
        }
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>();
        {
            RawMovementInput = context.ReadValue<Vector2>();

            NormInputX = Mathf.RoundToInt(RawMovementInput.x);
            NormInputY = Mathf.RoundToInt(RawMovementInput.y);

        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpInput = true;
            JumpInputStop = false;
            jumpInputStartTime = Time.time;
        }
        if (context.performed)
        {

        }
        if (context.canceled)
        {
            JumpInputStop = true;
        }
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            DashInput = true;
            DashInputStop = false;
            dashInputStartTime = Time.time;
        }
        if (context.canceled)
        {
            DashInputStop = true;
        }
    }

    public void OnWalkInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            WalkInput = true;
        }
        if (context.canceled)
        {
            WalkInput = false;
        }
    }

    public void OnMouseInput(InputAction.CallbackContext context)
    {
        MousePosition = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Vector2 aimDirection = MousePosition - armPosition;

        if (MousePosition.x >= armPosition.x)
        {
            MouseSide = 1;
        }
        else if((MousePosition.x < armPosition.x))
        {
            MouseSide = -1;
        }

        if(MouseSide == 1)
        {
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg + angleOffSet;
            armRB.MoveRotation(angle);
        }
        else if (MouseSide == -1)
        {
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg + angleOffSetFaceLeft;
            armRB.MoveRotation(angle);
        }

        
        
    }

    public void UseJumpInput() => JumpInput = false;

    public void UseDashInput() => DashInput = false;

    private void CheckJumpInputHoldTime()
    {
        if (Time.time >= jumpInputStartTime + inputHoldTime)
        {
            JumpInput = false;
        }
    }

    private void CheckDashInputHoldTime()
    {
        if (Time.time >= dashInputStartTime + inputHoldTime)
        {
            DashInput = false;
        }
    }
}

public enum CombatInputs
{
    primary,
    secondary
}