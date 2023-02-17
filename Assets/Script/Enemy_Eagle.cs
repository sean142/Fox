using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Eagle : Enemy
{
    private Rigidbody2D rb;
    //private Collider2D coll;
    public float speed;
    public Transform top, bottom;
    private float TopY, BottomY;

    private bool isup;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        //coll = GetComponent<Collider2D>();
        TopY = top.position.y;
        BottomY = bottom.position.y;
        Destroy(top.gameObject);
        Destroy(bottom.gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (isup)
        {
            rb.velocity = new Vector2(rb.velocity.x, speed);
            if (transform.position.y > TopY)
            {
                isup = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, -speed);
            if(transform.position.y < BottomY)
            {
                isup = true;
            }
        }
    }
}
