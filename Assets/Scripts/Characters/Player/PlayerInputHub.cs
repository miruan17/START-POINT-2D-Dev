using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHub : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction attackAction;
    private InputAction[] skillAction = new InputAction[4];
    private System.Action<InputAction.CallbackContext>[] skillHandlers = new System.Action<InputAction.CallbackContext>[4];


    public Vector2 MoveInput { get; private set; }

    public bool flip = true;
    [SerializeField] private float deadzone = 0.1f;
    private bool facingRight = true;


    private bool jumpRequested;
    private bool attackRequested;
    private bool attackReleasedRequested;
    public bool AttackPressed { get; private set; }
    private bool[] skillRequested = new bool[4];


    // input request
    public bool JumpRequest()
    {
        if (!jumpRequested) return false;
        jumpRequested = false;
        return true;
    }
    public bool SkillRequest(int idx)
    {
        if (!skillRequested[idx]) return false;
        skillRequested[idx] = false;
        return true;
    }

    public bool AttackRequest()
    {
        if (!attackRequested) return false;
        attackRequested = false;
        return true;
    }

    public bool AttackReleaseRequest()
    {
        if (!attackReleasedRequested) return false;
        attackReleasedRequested = false;
        return true;
    }

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        attackAction = playerInput.actions["Attack"];
        for (int i = 0; i < 4; i++)
        {
            skillAction[i] = playerInput.actions[$"Skill {i + 1}"];
        }
        // flip Awake
        var s = transform.localScale;
        s.y = Mathf.Abs(s.y);
        s.z = 1f;
        transform.localScale = s;
    }

    private void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
        attackAction.Enable();
        for (int i = 0; i < 4; i++)
        {
            skillAction[i].Enable();
        }
        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;
        jumpAction.started += OnJump;
        for (int i = 0; i < 4; i++)
        {
            int index = i;
            skillHandlers[i] = (ctx) => OnSkill(ctx, index);
            skillAction[i].performed += skillHandlers[i];
        }

        attackAction.started += OnAttackStarted;
        attackAction.canceled += OnAttackCanceled;
    }

    private void OnDisable()
    {
        moveAction.performed -= OnMove;
        moveAction.canceled -= OnMove;
        jumpAction.started -= OnJump;
        for (int i = 0; i < 4; i++)
        {
            if (skillHandlers[i] != null)
                skillAction[i].performed -= skillHandlers[i];
        }


        attackAction.started -= OnAttackStarted;
        attackAction.canceled -= OnAttackCanceled;

        moveAction.Disable();
        jumpAction.Disable();
        attackAction.Disable();
        for (int i = 0; i < 4; i++)
        {
            skillAction[i].Disable();
        }
    }

    private void Update()
    {
        if (!flip) return;

        float x = MoveInput.x;

        if (x > deadzone && !facingRight) ApplyFlip(true);
        else if (x < -deadzone && facingRight) ApplyFlip(false);
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext ctx)
    {
        jumpRequested = true;
    }

    private void OnAttackStarted(InputAction.CallbackContext ctx)
    {
        AttackPressed = true;
        attackRequested = true;
    }

    private void OnAttackCanceled(InputAction.CallbackContext ctx)
    {
        AttackPressed = false;
        attackReleasedRequested = true;
    }

    private void OnSkill(InputAction.CallbackContext ctx, int idx)
    {
        Debug.Log(idx);
        skillRequested[idx] = true;
    }

    private void ApplyFlip(bool toRight)
    {
        facingRight = toRight;

        var s = transform.localScale;
        s.x = toRight ? Mathf.Abs(s.x) : -Mathf.Abs(s.x);
        s.y = Mathf.Abs(s.y);
        s.z = 1f;
        transform.localScale = s;
    }
}
