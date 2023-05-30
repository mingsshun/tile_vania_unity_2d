using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(10f, 10f);
    Vector2 moveInput;
    Rigidbody2D rigidbody2d;
    Animator animator;
    CapsuleCollider2D bodyCollider2D;
    BoxCollider2D feetCollider2D;
    float gravityScaleAtStart;
    bool isAlive = true;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bodyCollider2D = GetComponent<CapsuleCollider2D>();
        feetCollider2D = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = rigidbody2d.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAlive) return;
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    private void ClimbLadder()
    {
        if(!feetCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            rigidbody2d.gravityScale = gravityScaleAtStart;
            animator.SetBool("isClimbing", false);
            return;
        }
        rigidbody2d.gravityScale = 0f;
        Vector2 climbVelocity = new Vector2(rigidbody2d.velocity.x, moveInput.y * climbSpeed);
        rigidbody2d.velocity = climbVelocity;

        animator.SetBool("isClimbing", Mathf.Abs(rigidbody2d.velocity.y) > Mathf.Epsilon);
    }

    void OnMove(InputValue value)
    {
        if(!isAlive) return;
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }
    void OnJump(InputValue value)
    {
        if(!isAlive) return;
        if(!feetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
            return;
        if(value.isPressed)
        {
            rigidbody2d.velocity += new Vector2(0f, jumpSpeed);
        }
    }
    private void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, rigidbody2d.velocity.y);
        rigidbody2d.velocity = playerVelocity;

        animator.SetBool("isRunning", Mathf.Abs(rigidbody2d.velocity.x) > Mathf.Epsilon);
    }
    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rigidbody2d.velocity.x) > Mathf.Epsilon;
        if(playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rigidbody2d.velocity.x), 1f);
        }
    }

    private void Die()
    {
        if(bodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            isAlive = false;
            animator.SetTrigger("Dying");
            rigidbody2d.velocity = new Vector2(deathKick.x * -(transform.localScale.x), deathKick.y);
        }
    }
}
