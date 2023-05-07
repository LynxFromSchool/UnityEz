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


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        if(Mathf.Abs(horizontal) == 1)
        {
           rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x / slide, rb.velocity.y);
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumps != 0)
        {
            jumps -= 1;
            // this is so if you double jump the jump isnt small and its the same as the normal one
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector3.up * JumpPower * 100);
        }

    }
    private void OnCollisionEnter2D(Collision2D other) {
        jumps = maxjumps;
    }
}
