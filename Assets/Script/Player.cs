using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private Rigidbody2D rd;
    private Animator anim;

    public Transform CellingCheck,GroundCheck;
    public AudioSource jumpAudio,hurtAudio,cherryAudio;
    public Collider2D coll;
    public Collider2D Discoll;
    public LayerMask ground;
    public float speed;
    public float jumpfarce;
    public int Cherry;

    public Text CherryNum;
    private bool isHurt;
    private bool isGround,isJump;
    private int extraJump;

    bool jumpPressed;
    int jumpCount;

    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        isGround = Physics2D.OverlapCircle(GroundCheck.position, 0.1f, ground);
        if (!isHurt)
        {
            Movement();
        }             
        SwitchAnim();
        newJump();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
            jumpPressed = true;
        Crouch();
        //Jump();
        CherryNum.text = Cherry.ToString();
     
    }

    void Movement()
    {
        float horiztalmove = Input.GetAxisRaw("Horizontal");
        rd.velocity = new Vector2(horiztalmove * speed, rd.velocity.y);

        if (horiztalmove != 0)
        {
            transform.localScale = new Vector3(horiztalmove, 1, 1);
        }

    }

    //切換動畫效果
    void SwitchAnim()
    {
        //anim.SetBool("idle", false);

        anim.SetFloat("running", Mathf.Abs(rd.velocity.x));
        if (isGround)
        {
            anim.SetBool("falling", false);
        }
        else if (isHurt)
        {
            anim.SetBool("hurt", true);
            anim.SetFloat("running", 0);
            if (Mathf.Abs(rd.velocity.x) < 0.1f)
            {
                anim.SetBool("hurt", false);
                //anim.SetBool("idle", true);
                isHurt = false;
            }
        }
        else if (!isGround && rd.velocity.y > 0)
        {
            anim.SetBool("jumping", true);
        }
        else if (rd.velocity.y < 0)
        {
            anim.SetBool("jumping", false);
            anim.SetBool("falling", true);
        }
    }
   
    //碰撞觸發器
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //收集物品
        if (collision.tag == "Collection")
        {
            // cherryAudio.Play();
            SoundManager.instance.CherryAudio();
            //Destroy(collision.gameObject);
            collision.GetComponent<Animator>().Play("isCot");
            //Cherry += 1;   
            //CherryNum.tag = Cherry.ToString();
        }
        if (collision.tag == "DeadLine")
        {
            //GetComponent<AudioSource>().enabled = false;
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
                rd.velocity = new Vector2(rd.velocity.x, jumpfarce);
                anim.SetBool("jumping", true);
            }
            else if (transform.position.x < collision.gameObject.transform.position.x) 
            {
                rd.velocity = new Vector2(-3, rd.velocity.y);
                //hurtAudio.Play();
                SoundManager.instance.HurtAudio();
                isHurt = true;
            }
            else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                rd.velocity = new Vector2(3, rd.velocity.y);
                //hurtAudio.Play(); 
                SoundManager.instance.HurtAudio();
                isHurt = true;
            }
        }
    } 
    
    //蹲下趴下
    void Crouch()
    {
        if (!Physics2D.OverlapCircle(CellingCheck.position, 0.2f, ground))
        {
            if (Input.GetButton("Crouch"))
            {
                anim.SetBool("crouching", true);
                Discoll.enabled = false;
            }
            else 
            {
                anim.SetBool("crouching", false);
                Discoll.enabled = true;
            }
        }        
    }

    //腳色跳躍
    /*void Jump()
    {
        if (Input.GetButton("Jump") && coll.IsTouchingLayers(ground))
        {
            rd.velocity = new Vector2(0, jumpfarce);
            jumpAudio.Play();
            anim.SetBool("jumping", true);
        }
    }*/

    void newJump()
    {
        if (isGround)
        {
            jumpCount = 2;
            isJump = false;
        }
        if (jumpPressed && isGround)
        {
            isJump = true;
            rd.velocity = new Vector2(rd.velocity.x, jumpfarce);
            jumpCount--;
            jumpPressed = false;

            //gameObject.GetComponent<AudioSource>().PlayOneShot(Jump);
        }
        else if (jumpPressed && jumpCount > 0 && isJump)
        {
            rd.velocity = new Vector2(rd.velocity.x, jumpfarce);
            jumpCount--;
            jumpPressed = false;

            //gameObject.GetComponent<AudioSource>().PlayOneShot(Jump);

        }
    }

    //重製當前場景
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CherryCount()
    {
        Cherry += 1;
    }
}
