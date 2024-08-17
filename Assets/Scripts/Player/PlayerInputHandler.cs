using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private Camera cam;
    public Vector2 RawMovementInput{get;private set;}
    public Vector2 RawDashDirectionInput{get;private set;}
    public Vector2Int DashDirectionInput{get;private set;}
    public int NormInputX{get;private set;}
    public int NormInputY{get;private set;}
    public bool JumpInput{get;private set;}
    public bool JumpInputStop{get;private set;}
    public bool GrabInput{get;private set;}
    public bool DashInput{get;private set;}
    public bool DashInputStop{get;private set;}

    [SerializeField]
    private float inputHoldTime;
    private float jumpStartTime;
    private float dashStartTime;

    private void Start()
    {
        playerInput=GetComponent<PlayerInput>();
        cam=Camera.main;
    }

    private void Update()
    {
        CheckJumpInputHoldTime();
        CheckDashInputHoldTime();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput=context.ReadValue<Vector2>();
        NormInputX=(int)(RawMovementInput.x*Vector2.right).normalized.x;
        NormInputY=(int)(RawMovementInput.y*Vector2.up).normalized.y;
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            JumpInput=true;
            JumpInputStop=false;
            jumpStartTime=Time.time;
        }
        else if(context.canceled)
        {
            JumpInputStop=true;
        }
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            DashInput=true;
            DashInputStop=false;
            dashStartTime=Time.time;
        }
        else if(context.canceled)
        {
            DashInputStop=true;
        }
    }

    public void OnDashDirectionInput(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            RawDashDirectionInput=context.ReadValue<Vector2>();
            if(playerInput.currentControlScheme=="Keyboard&Mouse")
            {
                RawDashDirectionInput=cam.ScreenToWorldPoint(RawDashDirectionInput)-transform.position;
            }
            DashDirectionInput=Vector2Int.RoundToInt(RawDashDirectionInput.normalized);
        }
    }

    public void OnGrab(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            GrabInput=true;
        }
        else if(context.canceled)
        {
            GrabInput=false;
        }
    }

    public void UseJumpInput()
    {
        JumpInput=false;
    }

    public void UseJumpInputStop()
    {
        JumpInputStop=false;
    }

    public void UseDashInput()
    {
        DashInput=false;
    }

    public void UseDashInputStop()
    {
        DashInputStop=false;
    }

    private void CheckJumpInputHoldTime()
    {
        if(Time.time>jumpStartTime+inputHoldTime)
        {
            JumpInput=false;
        }
    }

    private void CheckDashInputHoldTime()
    {
        if(Time.time>dashStartTime+inputHoldTime)
        {
            DashInput=false;
        }
    }
}
