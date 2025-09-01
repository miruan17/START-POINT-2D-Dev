using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float MaxSpeed = 5f;
    public float JumpPower = 7f;
    public LayerMask groundMask;
    public float groundCheckDepth = 0.1f;

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
        if (Input.GetButtonDown("Jump") && !anim.GetBool("Jump"))
        {
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
    }

    void FixedUpdate()
    {
        //Walk
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if (rigid.velocity.x > MaxSpeed)
            rigid.velocity = new Vector2(MaxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < -MaxSpeed)
            rigid.velocity = new Vector2(-MaxSpeed, rigid.velocity.y);

        //GroundCheck: OverlapBox
        Bounds b = bodyCol.bounds;
        Vector2 boxCenter = new Vector2(b.center.x, b.min.y - groundCheckDepth * 0.5f);
        Vector2 boxSize   = new Vector2(b.size.x * 0.9f, groundCheckDepth);

        bool isGrounded = Physics2D.OverlapBox(boxCenter, boxSize, 0f, groundMask);
        anim.SetBool("Jump", !isGrounded);
        Debug.DrawLine(new Vector2(boxCenter.x - boxSize.x/2, boxCenter.y),
                       new Vector2(boxCenter.x + boxSize.x/2, boxCenter.y), Color.green);
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
