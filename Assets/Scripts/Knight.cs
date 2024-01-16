using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Knight : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public float moveSpeed;
    public float jumpSpeed;
    public float moveInput;

    private bool isOnGround;
    public Transform playerPos;
    public float positionRadius;
    public LayerMask ground;

    private float airTimeCount;
    public float airTime;
    private bool inAir;

    public Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }


    void Update()
    {
        isOnGround = Physics2D.OverlapCircle(playerPos.position, positionRadius, ground);

        animator.SetFloat("Speed", Mathf.Abs(moveInput));

        if (moveInput > 0)
        {
            sr.flipX = true;
        }else if (moveInput < 0)
        {
            sr.flipX = false;
        }

        if (isOnGround == true && Input.GetKeyDown(KeyCode.Space))
        {
            inAir = true;
            airTimeCount = airTime;
            rb.velocity = Vector2.up * jumpSpeed;
            animator.SetBool("IsJumping", true);
        }

        if (Input.GetKey(KeyCode.Space) && inAir == true)
        {



            if (airTimeCount > 0)
            {
                rb.velocity = Vector2.up * jumpSpeed;
                airTimeCount -= Time.deltaTime;
            }
            else
            {
                inAir = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            inAir = false;
        }
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }
}
