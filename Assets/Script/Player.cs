using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player instance;

    private Rigidbody2D rd;
    private Animator anim;
    private Collider2D discoll;

    [Space(10)]
    public Transform cellingCheck,groundCheck;
    public LayerMask ground;

    [Space(10)]
    public float speed;
    public float jumpFarce;

    [Space(10)]
    public int cherry;
    public Text cherryNum;

    [Space(10)]
    bool jumpPressed;
    int jumpCount;

    [Space(10)]
    public bool isHurt;
    public bool isGround;
    public bool playEnd;
    public bool canMove;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;

        discoll = GetComponent<BoxCollider2D>();
        rd = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {   
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);

        if (!playEnd)
        {
            if (!isHurt && canMove)
            {
                Movement();
                Jump();            
                Crouch();                
            }
        }
        else
        {
            canMove = false;
        }
        SwitchAnim();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            jumpPressed = true;
        }

        cherryNum.text = cherry.ToString();
       
    }

    void Movement()
    {
        float horiztalMove = Input.GetAxisRaw("Horizontal");
        rd.velocity = new Vector2(horiztalMove * speed*Time.fixedDeltaTime, rd.velocity.y);

        if (horiztalMove != 0)
        {
            transform.localScale = new Vector3(horiztalMove, 1, 1);
        }       
    }

    //切換動畫效果
    void SwitchAnim()
    {
        anim.SetFloat("running", Mathf.Abs(rd.velocity.x));

        if(isGround && isHurt)
        {
            canMove = false;

        }   
        else if (isGround)
        {
            canMove = true;
            anim.SetBool("falling", false);
        }       
        else if (!isGround && rd.velocity.y > 0)
        {
            anim.SetBool("jumping", true);
            anim.SetBool("falling", false);
        }
        else if (rd.velocity.y < 0)
        {
            anim.SetBool("jumping", false);
            anim.SetBool("falling", true);
        }

        if (isHurt)
        {
            canMove = false;
            anim.SetBool("hurt", true);

            anim.SetFloat("running", 0);
            if (Mathf.Abs(rd.velocity.x) < 0.1f)
            {
                isHurt = false;
                anim.SetBool("hurt", false);
                canMove = true;
            }
        }
    }
   
    //碰撞觸發器
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //收集物品
        if (collision.tag == "Cherry")
        {
            SoundManager.instance.CherryAudio();
            collision.GetComponent<Animator>().Play("isCot");
            
        }
        if (collision.tag == "DeadLine")
        {
            Invoke("Restart", 2f);
        }
    }

    //消滅怪物
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (anim.GetBool("falling"))
            {
                enemy.JumpOn();
                rd.velocity = new Vector2(rd.velocity.x, jumpFarce);
                anim.SetBool("jumping", true);
            }
            else if (transform.position.x < collision.gameObject.transform.position.x) 
            {
                rd.velocity = new Vector2(-5, rd.velocity.y);
                SoundManager.instance.HurtAudio();
                isHurt = true;                
                canMove = false;
            }
            else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                rd.velocity = new Vector2(5, rd.velocity.y);
                SoundManager.instance.HurtAudio();
                isHurt = true;
                canMove = false;
            }
        }        
    } 
    
    //蹲下趴下
    void Crouch()
    {
        if (!Physics2D.OverlapCircle(cellingCheck.position, 0.2f, ground) )
        {
            if (Input.GetButton("Crouch") &&isGround )
            {
                anim.SetBool("crouching", true);
                discoll.enabled = false;
            }
            else 
            {
                anim.SetBool("crouching", false);
                discoll.enabled = true;
            }
        }        
    }

    void Jump()
    {
        if (isGround)
        {
            jumpCount = 2;
        }
        if (jumpPressed && isGround) 
        {
            rd.velocity = new Vector2(rd.velocity.x, jumpFarce);
            jumpCount--;
            jumpPressed = false;
            SoundManager.instance.JumpAudio();
        }
        else if (jumpPressed && jumpCount > 0)
        {
            rd.velocity = new Vector2(rd.velocity.x, jumpFarce);
            jumpCount--;
            jumpPressed = false;
            SoundManager.instance.JumpAudio();
        }
    }
    
    //重製當前場景
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CherryCount()
    {
        cherry += 1;
    }
}
