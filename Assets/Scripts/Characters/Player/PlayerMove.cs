using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float WalkSpeed = 0f;
    public float RunSpeed = 0f;
    public float JumpPower = 0f;
    public LayerMask groundMask;
    public float groundCheckDepth = 0f;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;
    Animator anim;
    Collider2D bodyCol;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        bodyCol = GetComponent<Collider2D>();
    }
    
    void Update()
    {
        //Jump
        if (Input.GetButton("Jump") && !anim.GetBool("Jump"))
        {
            rigid.velocity = new Vector2(rigid.velocity.x, 0f);
            rigid.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
            anim.SetBool("Jump", true);
        }

        // FlipX
        if (Input.GetAxisRaw("Horizontal") < 0)
            spriteRenderer.flipX = true;
        else if (Input.GetAxisRaw("Horizontal") > 0)
            spriteRenderer.flipX = false;

        //Walk Animation
        anim.SetBool("Walk", Mathf.Abs(rigid.velocity.x) > 0.05f);

        //Run Animation
        anim.SetBool("Run", Mathf.Abs(rigid.velocity.x) > WalkSpeed);
    }

    void FixedUpdate()
    {
        //GroundCheck: OverlapBox
        Bounds b = bodyCol.bounds;
        Vector2 boxCenter = new Vector2(b.center.x, b.min.y - groundCheckDepth * 0.5f);
        Vector2 boxSize = new Vector2(b.size.x * 0.9f, groundCheckDepth);

        bool isGrounded = Physics2D.OverlapBox(boxCenter, boxSize, 0f, groundMask);
        anim.SetBool("Jump", !isGrounded);
        Debug.DrawLine(new Vector2(boxCenter.x - boxSize.x / 2, boxCenter.y),
                       new Vector2(boxCenter.x + boxSize.x / 2, boxCenter.y), Color.green);

        //Walk & Run
        float h = Input.GetAxisRaw("Horizontal");
        if (isGrounded)
        {
            rigid.AddForce(Vector2.right * h * 2, ForceMode2D.Impulse);
        }

        if (!Input.GetKey(KeyCode.LeftShift) && isGrounded)
        {
            if (rigid.velocity.x > WalkSpeed)
                rigid.velocity = new Vector2(WalkSpeed, rigid.velocity.y);
            else if (rigid.velocity.x < -WalkSpeed)
                rigid.velocity = new Vector2(-WalkSpeed, rigid.velocity.y);
        }
        else
        {
            if (rigid.velocity.x > RunSpeed)
                rigid.velocity = new Vector2(RunSpeed, rigid.velocity.y);
            else if (rigid.velocity.x < -RunSpeed)
                rigid.velocity = new Vector2(-RunSpeed, rigid.velocity.y);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (!GetComponent<Collider2D>()) return;
        var b = GetComponent<Collider2D>().bounds;
        var center = new Vector2(b.center.x, b.min.y - groundCheckDepth * 0.5f);
        var size   = new Vector2(b.size.x * 0.9f, groundCheckDepth);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(center, size);
    }
}
