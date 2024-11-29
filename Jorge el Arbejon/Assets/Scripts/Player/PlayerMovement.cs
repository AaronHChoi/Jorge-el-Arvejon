using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Analytics;
using Unity.Services.Core;
using System.Linq;
using System;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    private enum MovementState { idle, running, jumping, falling, slide }

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Rigidbody2D rb;

    // Lateral movement
    private float horizontal;

    [SerializeField] private const float walkSpeed = 10f;
    private const float runSpeed = walkSpeed * 1.5f;
    private float moveSpeed = walkSpeed;

    // Vertical movement
    [SerializeField] private float jumpingPower = 15f;
    private bool isFacingRight = true;

    [SerializeField] private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    [SerializeField] private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    // Dash
    [SerializeField] private TrailRenderer tr;

    private bool canDash = true;
    private bool isDashing;
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;

    private float direction;

    // Wall slide and wall jump
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

    [SerializeField] private AudioSource jumpSoundEffect;
    [SerializeField] private AudioSource dashSoundEffect;
    private int dashCount = 0; // Track the total number of dashes

    void Update()
    {
        if (!PauseMenu.isPaused)
        {
            if (isDashing)
            {
                return;
            }

            Jump();

            WallSlide();

            WallJump();

            if (!isWallJumping)
            {
                Flip();
            }

            Run();

            if (Input.GetKey(KeyCode.LeftControl) && canDash)
            {
                dashSoundEffect.Play();
                StartCoroutine(Dash());
            }

            UpdateAnimationState();
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void FixedUpdate()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

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

    private int jumpCount = 0; // Track how many times the player has jumped

    private void Jump()
    {
        // Check if the player pressed the jump button and is grounded
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            jumpSoundEffect.Play();

            // Increment the jump count
            jumpCount++;

            // Send a custom analytics event
            SendJumpEvent();
        }

        // Handle jump release for jump height adjustment
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private async void SendJumpEvent()
    {
        try
        {
            // Ensure Unity Services are initialized
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                await UnityServices.InitializeAsync();
                Debug.Log("Unity Services Initialized Successfully");
            }

            // Create and configure the custom event
            PlayerJumpEvent playerJumpEvent = new PlayerJumpEvent
            {
                JumpCount = jumpCount // Pass the jump count
            };

            // Record the custom event
            AnalyticsService.Instance.RecordEvent(playerJumpEvent);

            Debug.Log($"PlayerJump event recorded successfully with jumpCount: {jumpCount}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error while recording PlayerJump event: {e.Message}");
        }
    }


    private async void SendDashEvent()
    {
        try
        {
            // Ensure Unity Services are initialized
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                await UnityServices.InitializeAsync();
                Debug.Log("Unity Services Initialized Successfully");
            }

            // Create and configure the PlayerDashEvent
            PlayerDashEvent playerDashEvent = new PlayerDashEvent
            {
                DashCount = dashCount // Pass the dash count
            };

            // Record the PlayerDash event
            AnalyticsService.Instance.RecordEvent(playerDashEvent);

            Debug.Log($"PlayerDash event recorded successfully with DashCount: {dashCount}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error while recording PlayerDash event: {e.Message}");
        }
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

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;

            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);

            wallJumpingCounter = 0f;

            if (horizontal != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                transform.Rotate(0f, 180f, 0f);

                jumpSoundEffect.Play();
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
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);
        }
    }

    private void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = runSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }
    }

    private IEnumerator Dash()
    {
        if (canDash)
        {
            canDash = false;
            dashCount++; // Increment the dash counter
            SendDashEvent(); // Record the dash event

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
