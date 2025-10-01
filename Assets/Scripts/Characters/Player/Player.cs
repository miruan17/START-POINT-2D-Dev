using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    // inputSystem
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction attackAction;

    public Vector2 MoveInput { get; private set; }

    private bool jumpRequested;
    private bool attackRequested;

    public bool ConsumeJumpRequest()
    {
        if (!jumpRequested) return false;
        jumpRequested = false;
        return true;
    }

    public bool ConsumeAttackRequest()
    {
        if (!attackRequested) return false;
        attackRequested = false;
        return true;
    }

    protected override void Awake()
    {
        // Character
        base.Awake();
        playerInput = GetComponent<PlayerInput>();

        moveAction   = playerInput.actions["Move"];
        jumpAction   = playerInput.actions["Jump"];
        attackAction = playerInput.actions["Attack"];
    }

    private void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
        attackAction.Enable();

        moveAction.performed += OnMove;
        moveAction.canceled  += OnMove;
        jumpAction.started   += OnJump;
        attackAction.started += OnAttack;
    }

    private void OnDisable()
    {
        moveAction.performed -= OnMove;
        moveAction.canceled  -= OnMove;
        jumpAction.started   -= OnJump;
        attackAction.started -= OnAttack; // ★ 추가

        moveAction.Disable();
        jumpAction.Disable();
        attackAction.Disable();           // ★ 추가
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext ctx)
    {
        jumpRequested = true;
    }

    private void OnAttack(InputAction.CallbackContext ctx)
    {
        attackRequested = true;
    }
}
