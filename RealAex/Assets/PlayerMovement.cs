using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    public float moveSpeed;
    public float JumpPower;
    public float maxjumps;
    float jumps;
    public float slide;
    public bool isFacingRight;
    public bool isFacingLeft;

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    [SerializeField] private TrailRenderer tr;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.S))
        {
            rb.velocity = new Vector2(rb.velocity.x, -15);
        }
        if(Input.GetKeyUp(KeyCode.S))
        {
            rb.velocity = new Vector2(rb.velocity.x, -8);
        }
        if (isDashing)
        {
            return;
        }
        //walking
        float horizontal = Input.GetAxisRaw("Horizontal");
        if(Mathf.Abs(horizontal) == 1)
        {
           rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
           if(horizontal == 1)
           {
            isFacingRight = true;
            isFacingLeft = false;
           }
           else if(horizontal == -1)
           {
            isFacingLeft = true;
            isFacingRight = false;
           }
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x / slide, rb.velocity.y);
        }
        //Jumping
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(jumps == 0)
            {
                StartCoroutine(jumpDelay());
            }
            else
            {
                Jump();
            }

        }
        //dashing
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

    }
    private void OnCollisionEnter2D(Collision2D other) {
        jumps = maxjumps;
    }
    private void Jump()
    {
        jumps -= 1;
        // this is so if you double jump the jump isnt small and its the same as the normal one
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector3.up * JumpPower * 100);
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        if(isFacingRight)
        {
            rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        }
        else
        {
             rb.velocity = new Vector2(-transform.localScale.x * dashingPower, 0f);
        }
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    private IEnumerator jumpDelay()
    {
        // this is so if you have no jumps and u click jump, if in the next 0.12 seconds you get your jumps it jumps for
        yield return new WaitForSeconds(0.12f);
        if(jumps != 0)
        {
            Jump();
        }
    }
}
