using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Player player;
    private PlayerInputHub input;
    private Rigidbody2D rigid;
    private Animator anim;
    private Collider2D bodyCol;

    [Header("Speed Scale")]
    public float speedScale = 1.0f;
    private float Speed;

    private float accel = 1.0f;
    private float deccel = 1.0f;

    [Header("Jump")]
    public float jumpPower = 13f;

    [Header("Ground Check")]
    public float groundCheckDepth = 0.1f;
    public LayerMask groundMask;

    public bool isGrounded;

    private void Awake()
    {
        player = GetComponent<Player>();
        input = GetComponent<PlayerInputHub>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        bodyCol = GetComponent<Collider2D>();
    }

    private void Update()
    {
        Speed = speedScale * player.FinalSpd;

        anim.SetBool("Move", Mathf.Abs(rigid.velocity.x) > 0.01f);
    }

    private void FixedUpdate()
    {
        // ground check
        isGrounded = OverlapGround();
        anim.SetBool("Jump", !isGrounded);

        // move
        float h = Mathf.Clamp(input.MoveInput.x, -1f, 1f);
        if (isGrounded)
        {
            rigid.AddForce(Vector2.right * h * 2f, ForceMode2D.Impulse);
        }

        // speed maximum
        float vx = Mathf.Clamp(rigid.velocity.x, -Speed, Speed);
        rigid.velocity = new Vector2(vx, rigid.velocity.y);

        // jump
        if (input.JumpRequest() && isGrounded && !anim.GetBool("Jump"))
        {
            rigid.velocity = new Vector2(rigid.velocity.x, 0f);
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("Jump", true);
        }
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

    private void OnDrawGizmosSelected()
    {
        if (!bodyCol) bodyCol = GetComponent<Collider2D>();
        if (!bodyCol) return;

        var b = bodyCol.bounds;
        var center = new Vector2(b.center.x, b.min.y - groundCheckDepth * 0.5f);
        var size   = new Vector2(b.size.x * 0.9f, groundCheckDepth);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(center, size);
    }
}
