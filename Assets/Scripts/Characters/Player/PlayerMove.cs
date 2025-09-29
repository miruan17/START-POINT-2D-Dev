using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMove : Character
{
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction runAction;

    private Vector2 moveInput;

    private float WalkSpeed = 0f;
    private float RunSpeed  = 0f;
    [Header("JumpPower")]
    public float JumpPower = 0f;

    [Header("Ground")]
    public float groundCheckDepth = 0.1f;
    public LayerMask groundMask;

    private bool isGrounded;

    protected override void Awake()
    {
        base.Awake();
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        runAction  = playerInput.actions["Run"];
    }

    void OnEnable()
    {
        moveAction.performed += OnMove;
        moveAction.canceled  += OnMove;
        jumpAction.started   += OnJump;
    }

    void OnDisable()
    {
        moveAction.performed -= OnMove;
        moveAction.canceled  -= OnMove;
        jumpAction.started   -= OnJump;
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext ctx)
    {
        if (isGrounded && !anim.GetBool("Jump"))
        {
            rigid.velocity = new Vector2(rigid.velocity.x, 0f);
            rigid.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
            anim.SetBool("Jump", true);
        }
    }

    void Update()
    {
        // FlipX
        if (moveInput.x < 0) spriteRenderer.flipX = true;
        else if (moveInput.x > 0) spriteRenderer.flipX = false;

        // Walk / Run 애니메이션
        anim.SetBool("Walk", Mathf.Abs(rigid.velocity.x) > 0.05f);
        anim.SetBool("Run",  Mathf.Abs(rigid.velocity.x) > WalkSpeed);
    }

    void FixedUpdate()
    {
        WalkSpeed = FinalSpd;
        RunSpeed = WalkSpeed * 2;
        // Ground Check
        isGrounded = OverlapGround();
        anim.SetBool("Jump", !isGrounded);

        // 좌우 가속(지면에서만)
        float h = Mathf.Clamp(moveInput.x, -1f, 1f);
        if (isGrounded)
            rigid.AddForce(Vector2.right * h * 2f, ForceMode2D.Impulse);

        // 달리기 입력
        bool isRunPressed = runAction != null && runAction.IsPressed();

        float vx = rigid.velocity.x;

        if (isGrounded)
        {
            float groundMax = isRunPressed ? RunSpeed : WalkSpeed;
            if (vx >  groundMax) vx =  groundMax;
            if (vx < -groundMax) vx = -groundMax;
        }
        else
        {
            float airHardCap = RunSpeed;
            if (vx >  airHardCap) vx =  airHardCap;
            if (vx < -airHardCap) vx = -airHardCap;
        }

        rigid.velocity = new Vector2(vx, rigid.velocity.y);
    }

    private bool OverlapGround()
    {
        if (!bodyCol) return false;

        Bounds b = bodyCol.bounds;
        Vector2 boxCenter = new Vector2(b.center.x, b.min.y - groundCheckDepth * 0.5f);
        Vector2 boxSize   = new Vector2(b.size.x * 0.9f, groundCheckDepth);

        bool hit = Physics2D.OverlapBox(boxCenter, boxSize, 0f, groundMask);

        Debug.DrawLine(new Vector2(boxCenter.x - boxSize.x / 2f, boxCenter.y),
                       new Vector2(boxCenter.x + boxSize.x / 2f, boxCenter.y),
                       hit ? Color.green : Color.red);

        return hit;
    }

    void OnDrawGizmosSelected()
    {
        var col = GetComponent<Collider2D>();
        if (!col) return;

        var b = col.bounds;
        var center = new Vector2(b.center.x, b.min.y - groundCheckDepth * 0.5f);
        var size   = new Vector2(b.size.x * 0.9f, groundCheckDepth);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(center, size);
    }
}
