using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public enum DeviceType { MouseAndKeyboard, Gamepad }
class InputController : Singleton<InputController>
{
    [HideInInspector]
    public DeviceType deviceType;

    [HideInInspector]
    public InputActions inputActions;
    [HideInInspector]
    public PlayerInput playerInput;
    void Awake()
    {
        inputActions = new InputActions();
        playerInput = GetComponent<PlayerInput>();

        SchemeChanged(playerInput.currentControlScheme);
    }

    private void OnEnable()
    {
        inputActions.Enable();
        InputUser.onChange += OnUserChange;
    }
    private void OnDisable()
    {
        inputActions.Disable();
        InputUser.onChange -= OnUserChange;
    }

    void OnUserChange(InputUser user, InputUserChange change, InputDevice device)
    {
        if (change == InputUserChange.ControlSchemeChanged)
        {
            //Debug.Log("Control scheme changed");
            SchemeChanged(user.controlScheme.Value.name);
        }
    }
    void SchemeChanged(string schemeName)
    {
        //Debug.Log("Control scheme changed: " + schemeName);
        if (schemeName == "Keyboard&Mouse")
        {
            deviceType = DeviceType.MouseAndKeyboard;
        }
        else
        {
            deviceType = DeviceType.Gamepad;
        }
    }

    public void EnableAllInput()
    {
        inputActions.Player.Move.Enable();
        inputActions.Player.MoveLeft.Enable();
        inputActions.Player.MoveRight.Enable();

        inputActions.Player.Crouch.Enable();
        inputActions.Player.Jump.Enable();
        inputActions.Player.Look.Enable();

        inputActions.UI._1.Enable();
        inputActions.UI._2.Enable();
        inputActions.UI._3.Enable();

        //inputActions.Touch.PrimaryTouch.Enable();
        //inputActions.Touch.PrimaryTouchPosition.Enable();
    }
    public void DisableAllInput()
    {
        inputActions.Player.Move.Disable();
        inputActions.Player.MoveLeft.Disable();
        inputActions.Player.MoveRight.Disable();

        inputActions.Player.Crouch.Disable();
        inputActions.Player.Jump.Disable();
        inputActions.Player.Look.Disable();

        inputActions.UI._1.Disable();
        inputActions.UI._2.Disable();
        inputActions.UI._3.Disable();

        //inputActions.Touch.PrimaryTouch.Disable();
        //inputActions.Touch.PrimaryTouchPosition.Disable();
    }


}