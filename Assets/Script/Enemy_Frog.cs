using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Frog : Enemy
{
    private Rigidbody2D rb;
    private  Collider2D coll;

    public LayerMask ground;
    public float speed,jumpForce;
    public Transform liftpoint, rightpoint;

    private bool Facelift = true;
    private float liftx, rightx;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();

        transform.DetachChildren();
        liftx = liftpoint.position.x;
        rightx = rightpoint.position.x;
        Destroy(liftpoint.gameObject);
        Destroy(rightpoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        SwitchAnim();      
    }
    
    public void Movement()
    {
        if (Facelift)//面左
        {
            if (coll.IsTouchingLayers(ground))
            {
                anim.SetBool("jumping", true);
                rb.velocity = new Vector2(-speed, jumpForce);
            }
            if (transform.position.x < liftx)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                Facelift = false;
            }
        }
        else//面右
        {
            if (coll.IsTouchingLayers(ground))
            {
                anim.SetBool("jumping", true);
                rb.velocity = new Vector2(speed, jumpForce);
            }
            if (transform .position.x > rightx)
            {
                transform.localScale = new Vector3(1, 1, 1);
                Facelift = true;
            }
        }
    }
    
    void SwitchAnim()
    {
        if (anim.GetBool("jumping"))
        {
            if(rb.velocity.y < 0.1)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        }
        if (coll.IsTouchingLayers(ground) && anim.GetBool("falling"))
        {
            anim.SetBool("falling", false);
        }
    }

    
    
}
