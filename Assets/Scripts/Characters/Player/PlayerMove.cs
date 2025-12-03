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

    [Header("Run Acceleration")]
    public float groundAccelAmount = 10f;   // 지상 가속
    public float groundDecelAmount = 15f;   // 지상 감속
    public float airAccelAmount = 6f;       // 공중 가속
    public float airDecelAmount = 8f;       // 공중 감속

    
    [Header("Custom Gravity")]
    public float gravityScale = 1.0f;        // 기본 중력
    public float fallGravityScale = 1.5f;    // 하강 중 강화된 중력
    public float maxFallSpeed = -20f;        // 최대 낙하 속도


    [Header("Jump Assist")]
    public float coyoteTime = 0.1f;         // 코요테 타임 (초)
    public float jumpBufferTime = 0.1f;     // 점프 버퍼 (초)

    private float coyoteCounter;            // 남은 코요테 타임
    private float jumpBufferCounter;        // 남은 점프 버퍼 타임

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

        // 코요테 타임 업데이트
        if (isGrounded)
        {
            coyoteCounter = coyoteTime;
        }
        else
        {
            coyoteCounter -= Time.fixedDeltaTime;
        }

        // 점프 입력 버퍼링
        if (input.JumpRequest())
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.fixedDeltaTime;
        }

        // 착지 / 이탈 처리
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
            else
            {
                // 발판에서 그냥 떨어질 때도 점프 횟수 하나 소모
                jumpNum--;
            }
        }
        prevGrounded = isGrounded;

        if (anim.GetBool("ActionLock")) return;
        float h = Mathf.Clamp(input.MoveInput.x, -1f, 1f);
        float targetSpeed = h * Speed;

        // 현재 상황에 따른 가속/감속 계수 선택
        bool isAccelerating = Mathf.Abs(targetSpeed) > 0.01f;

        float accelRate;
        if (isAccelerating)
        {
            accelRate = isGrounded ? groundAccelAmount : airAccelAmount;
        }
        else
        {
            accelRate = isGrounded ? groundDecelAmount : airDecelAmount;
        }

        float lerpAmount = accelRate * Time.fixedDeltaTime;
        float newSpeed = Mathf.Lerp(rigid.velocity.x, targetSpeed, lerpAmount);

        newSpeed = Mathf.Clamp(newSpeed, -Speed, Speed);

        // 수평 속도 적용
        rigid.velocity = new Vector2(newSpeed, rigid.velocity.y);

        // 공중에서 최고 높이 기록 (착지 이펙트용)
        if (!isGrounded)
        {
            maxheight = Mathf.Max(maxheight, bodyCol.bounds.min.y);
        }

        // Jump
        bool canCoyoteJump = coyoteCounter > 0f;
        bool canAirJump = !isGrounded && jumpNum > 0;

        // 점프 버퍼 안에 있고, 점프 가능 상태면 점프 수행
        if (jumpBufferCounter > 0f && jumpNum >= 1 && (canCoyoteJump || canAirJump || isGrounded))
        {
            // 실제로 점프 실행하면 버퍼/코요테 소모
            jumpBufferCounter = 0f;
            coyoteCounter = 0f;

            rigid.velocity = new Vector2(rigid.velocity.x, 0f);
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);

            // 점프 VFX
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

        float gravityMultiplier = (rigid.velocity.y > 0f) ? gravityScale : fallGravityScale;

        Vector2 velocity = rigid.velocity;
        velocity.y += Physics2D.gravity.y * gravityMultiplier * Time.fixedDeltaTime;

        // 최대 낙하 속도 제한
        if (velocity.y < maxFallSpeed)
            velocity.y = maxFallSpeed;

        rigid.velocity = new Vector2(rigid.velocity.x, velocity.y);

    }

    private bool OverlapGround()
    {
        if (!bodyCol) return false;

        Bounds b = bodyCol.bounds;
        Vector2 boxCenter = new Vector2(b.center.x, b.min.y - groundCheckDepth * 0.5f);
        Vector2 boxSize = new Vector2(b.size.x * 0.9f, groundCheckDepth);

        bool hit = Physics2D.OverlapBox(boxCenter, boxSize, 0f, groundMask);
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
