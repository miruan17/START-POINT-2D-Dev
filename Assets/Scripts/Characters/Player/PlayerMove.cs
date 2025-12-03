using System;
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

    [Header("Jump")]
    public float jumpPower = 13f;
    public float jumpNum;

    [Header("Ground Check")]
    public float groundCheckDepth = 0.1f;
    public LayerMask groundMask;
    public bool isGrounded;
    public bool prevGrounded = false;
    public float maxheight = 0;

    private void Awake()
    {
        player = GetComponent<Player>();
        input = GetComponent<PlayerInputHub>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        bodyCol = GetComponent<Collider2D>();
        jumpNum = player.status.GetFinal(StatId.JP);
    }

    private void Update()
    {
        if (anim.GetBool("Dash")) anim.SetBool("ActionLock", true);
        else anim.SetBool("ActionLock", false);
        if (anim.GetBool("ActionLock")) return;
        Speed = speedScale * player.status.GetFinal(StatId.SPD);

        anim.SetBool("Move", Mathf.Abs(rigid.velocity.x) > 0.01f && isGrounded);
        anim.SetBool("Jump", !isGrounded);
    }

    private void FixedUpdate()
    {
        isGrounded = OverlapGround();
        if (isGrounded != prevGrounded)
        {
            if (isGrounded) // air -> ground
            {
                jumpNum = player.status.GetFinal(StatId.JP);
                // 일정 거리 이상 낙하 시 착지 이펙트
                float distance = Mathf.Abs(bodyCol.bounds.min.y - maxheight);
                if (distance >= 10f)
                {
                    Vector3 spawnPos = new Vector3(bodyCol.bounds.center.x, bodyCol.bounds.min.y, 0f);
                    var vfx = Instantiate(player.landingVFX, spawnPos, Quaternion.identity);
                    vfx.transform.SetParent(player.transform);
                    if (!input.facingRight)
                    {
                        Vector3 scale = vfx.transform.localScale;
                        scale.x *= -1;
                        vfx.transform.localScale = scale;
                    }
                }
                maxheight = bodyCol.bounds.min.y;

                // PlatformGroupID 갱신
                PlatformGroupID pid = player.FindPlatform();
                if (pid != null) player.currentPlatform = pid;
            }
            else // ground -> air
            {
                jumpNum--;
            }
        }
        prevGrounded = isGrounded;
        if (anim.GetBool("ActionLock")) return;

        // move
        float h = Mathf.Clamp(input.MoveInput.x, -1f, 1f);
        if (isGrounded)
        {
            rigid.AddForce(Vector2.right * h * 2f, ForceMode2D.Impulse);
        }
        else
        {
            rigid.AddForce(Vector2.right * h * 0.33f, ForceMode2D.Impulse);
            maxheight = Mathf.Max(maxheight, bodyCol.bounds.min.y);
        }

        // speed maximum
        float vx = Mathf.Clamp(rigid.velocity.x, -Speed, Speed);
        rigid.velocity = new Vector2(vx, rigid.velocity.y);

        // jump
        if (input.JumpRequest() && jumpNum >= 1)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, 0f);
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);

            // 점프VFX
            Vector3 spawnPos = new Vector3(bodyCol.bounds.center.x, bodyCol.bounds.min.y, 0f);
            var vfx = Instantiate(player.jumpVFX, spawnPos, Quaternion.identity);
            vfx.transform.SetParent(player.transform);
            if (!input.facingRight)
            {
                Vector3 scale = vfx.transform.localScale;
                scale.x *= -1;
                vfx.transform.localScale = scale;
            }

            jumpNum--;
        }
    }

    private bool OverlapGround()
    {
        if (!bodyCol) return false;

        Bounds b = bodyCol.bounds;
        Vector2 boxCenter = new Vector2(b.center.x, b.min.y - groundCheckDepth * 0.5f);
        Vector2 boxSize = new Vector2(b.size.x * 0.9f, groundCheckDepth);

        bool hit = Physics2D.OverlapBox(boxCenter, boxSize, 0f, groundMask);

        // Debug.DrawLine(new Vector2(boxCenter.x - boxSize.x / 2f, boxCenter.y),
        //                new Vector2(boxCenter.x + boxSize.x / 2f, boxCenter.y),
        //                hit ? Color.green : Color.red);

        return hit;
    }

    private void OnDrawGizmosSelected()
    {
        if (!bodyCol) bodyCol = GetComponent<Collider2D>();
        if (!bodyCol) return;

        var b = bodyCol.bounds;
        var center = new Vector2(b.center.x, b.min.y - groundCheckDepth * 0.5f);
        var size = new Vector2(b.size.x * 0.9f, groundCheckDepth);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(center, size);
    }
}
