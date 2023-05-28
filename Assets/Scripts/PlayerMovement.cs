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
    Vector2 moveInput;
    Rigidbody2D rigidbody2d;
    Animator animator;
    CapsuleCollider2D capsuleCollider2D;
    float gravityScaleAtStart;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        gravityScaleAtStart = rigidbody2d.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        FlipSprite();
        ClimbLadder();
    }

    private void ClimbLadder()
    {
        if(!capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            rigidbody2d.gravityScale = gravityScaleAtStart;
            return;
        }
        rigidbody2d.gravityScale = 0f;
        Vector2 climbVelocity = new Vector2(rigidbody2d.velocity.x, moveInput.y * climbSpeed);
        rigidbody2d.velocity = climbVelocity;
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }
    void OnJump(InputValue value)
    {
        if(!capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
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
}
