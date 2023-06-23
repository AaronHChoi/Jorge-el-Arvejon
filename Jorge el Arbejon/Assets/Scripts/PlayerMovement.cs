using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    private enum MovementState { idle, running, jumping, falling, slide}

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer; 
    [SerializeField] private Rigidbody2D rb;

    // movimiento lateral
    private float horizontal;

    [SerializeField] private const float walkSpeed = 10f;
    private const float runSpeed = walkSpeed * 1.5f;
    private float moveSpeed = walkSpeed;
    // movimiento vertical

    [SerializeField] private float jumpingPower = 15f;
    private bool isFacingRight = true;

    [SerializeField] private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    [SerializeField] private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    // dash
    [SerializeField] private TrailRenderer tr;

    private bool canDash = true;
    private bool isDashing;
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;

    private float direction;

    // wall slide   y wall jump

    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    private bool isWallSliding;
    [SerializeField] private float wallSlidingSpeed = 2f;
    private bool isWallJumping;
    private float wallJumpingDirection;
    [SerializeField] private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    [SerializeField] private float wallJumpingDuration = 0.1f;

    [SerializeField] private Vector2 wallJumpingPower = new Vector2(2f, 15f);

    // knockback settings

    public float KBforce;
    public float KBcounter;
    public float KBtotaltime;

    public bool KBfromRight;
    
    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.isPaused)
        {

            if (isDashing)
            {
                return;
            }


            if (IsGrounded())
            {
                coyoteTimeCounter = coyoteTime;

            }
            else
            {
                coyoteTimeCounter -= Time.deltaTime;
            }

            if (Input.GetButton("Jump"))
            {
                jumpBufferCounter = jumpBufferTime;
            }
            else
            {
                jumpBufferCounter -= Time.deltaTime;
            }

            if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);

                jumpBufferCounter = 0f;
            }

            if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);

                coyoteTimeCounter = 0f;

            }

            WallSlide();

            WallJump();

            if (!isWallJumping)
            {
                Flip();
            }

            Run();

            if (Input.GetKey(KeyCode.LeftControl) && canDash)
            {
                StartCoroutine(Dash());
            }

            UpdateAnimationState();

        }

    }

    private bool IsGrounded()
    {
        {
            return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        }
    }

    private void FixedUpdate()
    {
        if (KBcounter <= 0)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            if (KBfromRight == true)
            {
                rb.velocity = new Vector2(-KBforce, KBforce);
            }
            if (KBfromRight == false)
            {
                rb.velocity = new Vector2(KBforce, KBforce);
            }

            KBcounter -= Time.deltaTime;
        }

        if (isDashing)
        {
            return;
        }

        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
        }
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }


    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -horizontal;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }

        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        // aca fix wall jump por ahora esta bien pero no hace bien el wall jump para la derecha hay que cambiar el codigo que deje de usar
        // scale x y usar rotate 180
        // se uso horizzontal

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;

            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);

            wallJumpingCounter = 0f;

            if (horizontal != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                transform.Rotate(0f, 180f, 0f);
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight =  !isFacingRight;



            transform.Rotate(0f, 180f, 0f);
            
        }
    }

    private void Run()
    {
        if (Input.GetKey (KeyCode.LeftShift))
        {
            moveSpeed = runSpeed;
        }

        else
        {
            moveSpeed = walkSpeed;
        }
    }


    // aca cambiar que la direccion del dash valla por la direccion de que esta dando el pj y no por scale x
    // se arreglo cambiando el scale con horizzontal nose porque pero esta fixed
    private IEnumerator Dash()
    {
            canDash = false;
            isDashing = true;

            float originalGravity = rb.gravityScale;

            rb.gravityScale = 0f;

            rb.velocity = new Vector2(horizontal * dashingPower, 0f);

            tr.emitting = true;

            yield return new WaitForSeconds(dashingTime);

            tr.emitting = false;

            rb.gravityScale = originalGravity;

            isDashing = false;

            yield return new WaitForSeconds(dashingCooldown);

            canDash = true;
    }

    private void UpdateAnimationState()
    {
        MovementState state;

        if (horizontal > 0f)
        {
            state = MovementState.running;
        }
        else if (horizontal < 0f)
        {
            state = MovementState.running;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > 0.1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -0.1f)
        {
            state = MovementState.falling;
        }

        animator.SetInteger("state", (int)state);
    }
}
