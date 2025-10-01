using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Player player;
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private Collider2D bodyCol;

    private float walkSpeed;

    [Header("Jump")]
    public float jumpPower = 7f;

    [Header("Ground Check")]
    public float groundCheckDepth = 0.1f;
    public LayerMask groundMask;

    private bool isGrounded;

    private void Awake()
    {
        player = GetComponent<Player>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        bodyCol = GetComponent<Collider2D>();
    }

    private void Update()
    {
        walkSpeed = player.FinalSpd;

        float hx = player.MoveInput.x;
        if (hx < -Mathf.Epsilon) spriteRenderer.flipX = true;
        else if (hx >  Mathf.Epsilon) spriteRenderer.flipX = false;

        // run anim
        anim.SetBool("Move", Mathf.Abs(rigid.velocity.x) > 0.01f);
    }

    private void FixedUpdate()
    {
        // ground check
        isGrounded = OverlapGround();
        anim.SetBool("Jump", !isGrounded);

        // flix x
        float h = Mathf.Clamp(player.MoveInput.x, -1f, 1f);
        if (isGrounded)
        {
            rigid.AddForce(Vector2.right * h * 2f, ForceMode2D.Impulse);
        }

        // spped maximum
        float vx = Mathf.Clamp(rigid.velocity.x, -walkSpeed, walkSpeed);
        rigid.velocity = new Vector2(vx, rigid.velocity.y);

        // jump
        if (player.ConsumeJumpRequest() && isGrounded && !anim.GetBool("Jump"))
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
