using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public bool isSkill;//是否在技能中
    public BOW bow;//弓的脚本
    public int WeapenStyle = 2;
    public static Player player;
    public Rigidbody2D rb;
    public float speed = 5f;
    public float h = 5;
    public float g = 10f;
    float v;
    Vector2 inputpos;
    public static Vector2 transfor;
    public Vector3 Playertran;//玩家的朝向
    //以下为冲刺代码
    bool isdash = false;//是否冲刺
    float dashtime = 0.2f;
    float dashtimeleft, dashCD = 1f, dashLast = -10f;
    float dashspeed = 50f;
    int Dashdirection_X;
    float Dashdirection_Y;//冲刺用数据
    float Force = 5f;
    bool Wantdash = false;
    bool isAttack = false,isLadder=false,isClimbing=false;
    //以下为跳跃检测
    [Range(1, 10)]
    private float jumpSpeed = 8f;
    private bool moveJump;//判断是否按下跳跃
    public bool isGround;//判断是否在地面上
    public Transform groundCheck;//地面检测
    public LayerMask ground;
    //以下为跳跃优化
    public float fallMultiplier = 1000f;//大跳的重力
    public float lowJumpMultiplier = 600f;//小跳的重力
    public float fallMultiplierElse = 3f;//重力补足
    //以下为多段跳功能的实现
    public int jumpCount = 2;//跳跃次数
    private bool isJump;//表示跳跃状态
    //以下为子弹特效
    public GameObject destroyEffect;//子弹销毁的特效
    public GameObject attackEffect;//攻击到玩家的特效
    //以下为受击后无敌时间
    public float flashes;//闪烁次数
    public float duration;//闪烁周期
    private SpriteRenderer sr;//颜色
    //以下为远程敌人攻击到玩家时玩家受击后撤的功能实现
    public Transform player_0;//获取玩家的位置组件
    public Transform farAttackEnemy;//获取敌人的位置组件
    private float BeattackTime = 0.2f;//受击时长
    bool beattack = false;
    int direction_e_p = 0;
    //以下为角色动画
    public Animator animator;//角色的状态机
    //以下为角色武器
    public GameObject hitbox;//徒手近战
    //以下为攻击计时器
    public float attackTimer;//攻击时间

    private void Awake()
    {
        player = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        v = Mathf.Sqrt(2 * h / g);
        rb = gameObject.GetComponent<Rigidbody2D>();
        inputpos = new Vector2();
        Playertran = rb.transform.localScale;
        sr = gameObject.GetComponent<SpriteRenderer>();//颜色组件
        hitbox.SetActive(false);//禁用武器
    }
    void Update()
    {
        transfor = this.gameObject.transform.position;
        if (!beattack && !isSkill)
        {
            if (!isdash)//玩家行动在这里面写0.0
            {
                PlayerJumpByTwice();
                MoveObject();
                Climb();
                if (Input.GetKeyDown(KeyCode.J))//攻击
                {
                    isAttack = true;
                    NormalAttack();//正在攻击

                }
                else
                {
                    //isAttack = false;
                    //animator.SetBool("IsAttack", false);//攻击动画的转向
                    //hitbox.SetActive(false);//启用武器
                }
            }
            if (Input.GetKeyDown(KeyCode.L))
                if (Time.time >= (dashLast + dashCD))
                    Wantdash = true;
            if (Wantdash)
                Dash();
        }
        if (beattack)
        {
            if (BeattackTime == 0.2f)
                //PlayerBeAttacking();
                BeattackTime -= Time.deltaTime;
            if (BeattackTime <= 0)
            {
                beattack = false;
                BeattackTime = 0.2f;
            }
        }
    }
    private void FixedUpdate()//固定为每秒50次检测的固定补足更新
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);//地面检测
    }
    public void MoveObject()//检测玩家的朝向的基础移动
    {
        inputpos = rb.velocity;
        inputpos.x = Input.GetAxisRaw("Horizontal") * speed;
        animator.SetFloat("SpeedX", Mathf.Abs(inputpos.x));//行走动画的转向
        rb.velocity = inputpos;
        if (inputpos.x < 0)
        {
            Playertran.x = -Mathf.Abs(Playertran.x);
            Dashdirection_X = -1;
            if (Input.GetAxisRaw("Vertical") == 0)
                Dashdirection_Y = 0;
        }
        if (inputpos.x > 0)
        {
            Playertran.x = Mathf.Abs(Playertran.x);
            Dashdirection_X = 1;
            if (Input.GetAxisRaw("Vertical") == 0)
                Dashdirection_Y = 0;
        }
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            Dashdirection_Y = 0.6f;
            if (inputpos.x == 0)
                Dashdirection_X = 0;
        }
        if (Input.GetAxisRaw("Vertical") < 0)
        {
            Dashdirection_Y = -0.6f;
            if (inputpos.x == 0)
                Dashdirection_X = 0;
        }
        rb.transform.localScale = Playertran;
    }
    void Dash()//冲刺完整代码
    {
        {
            if (Time.time >= (dashLast + dashCD))
            {
                Dashready();
            }
            Rush();
        }
        if (isdash)
        {
            dashtimeleft -= Time.deltaTime * 2;
            ShadowPool.instance.GetFormPool();//调用对象池
            if (dashtimeleft <= 0)
            {
                isdash = false;
                Wantdash = false;
                Invoke("LayerChange", 0.5f);
                rb.velocity = new Vector2(0, 0);
                rb.AddForce(new Vector2(Dashdirection_X, Dashdirection_Y) * 12f, ForceMode2D.Impulse);
                Dashdirection_Y = 0;
            }
        }
    }
    void Rush()//冲刺
    {
        if (isdash)
            if (dashtimeleft >= 0)
            //transform.Translate(transform.right * Time.deltaTime * dashtime*Dashdirection*dashspeed);
            {
                this.gameObject.layer = 9;
                rb.velocity = dashspeed * new Vector2(Dashdirection_X, Dashdirection_Y);
            }
    }
    void Dashready()//冲刺准备
    {
        isdash = true;
        dashtimeleft = dashtime;
        dashLast = Time.time;
    }
    void PlayerJumpByTwice()//二段跳
    {
        moveJump = Input.GetButtonDown("Jump");
        animator.SetFloat("SpeedY", inputpos.y);//跳跃动画的转向
        animator.SetBool("IsJump", true);//跳跃动画的转向
        JumpDetectionByTwice();
        //我是分界线，以下为优化跳跃手感内容
        if (Input.GetButtonDown("Jump") && rb.velocity.y < 0 && jumpCount > 0)
        {
            rb.velocity = Vector2.up * 9;
        }
        else if (rb.velocity.y < 0 && !Input.GetButtonDown("Jump"))
        {
            if (jumpCount > 0) rb.velocity += Vector2.up * Physics2D.gravity.y * fallMultiplier * Time.deltaTime;
            if (jumpCount == 0) rb.gravityScale = fallMultiplier;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.gravityScale = lowJumpMultiplier;
        }
        else
        {
            rb.gravityScale = 1.5f;
        }
    }
    void JumpDetectionByTwice()//二段跳检测
    {
        if (moveJump && jumpCount > 0)
        {
            isJump = true;
        }
        if (isGround)//判断是否在地面
        {
            jumpCount = (int)2f;//四舍五入为2
            animator.SetBool("IsJump", false);//跳跃动画的转向
        }
        if (isJump)
        {
            jumpCount--;//这里有点问题，第一次跳跃时无法检测到跳跃，二段跳时才能检测到，如果要使用二段跳动画的话直接在这里加上调用二段跳动画就可以了
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            jumpCount--;
            isJump = false;
        }
    }
    IEnumerator BeAttackedInvincibleTime()//受到攻击后的无敌时间
    {
        Physics2D.IgnoreLayerCollision(6, 7, true);//与敌人碰撞时
        Physics2D.IgnoreLayerCollision(6, 10, true);//与子弹碰撞时
        sr.color = Color.red;
        yield return new WaitForSeconds(duration / flashes);
        for (int i = 0; i < flashes; i++)
        {
            sr.color = new Color(255 / 255, 255 / 255, 255 / 255, 0.6f);//透明度变化
            yield return new WaitForSeconds(duration / flashes);
            sr.color = Color.white;
            yield return new WaitForSeconds(duration / flashes);
        }
        Physics2D.IgnoreLayerCollision(6, 7, false);
        Physics2D.IgnoreLayerCollision(6, 10, false);
    }

    private void NormalAttack()//普通公鸡
    {
        animator.SetBool("IsAttack", true);//攻击动画的转向
        if (WeapenStyle == 1)
        {
            Debug.Log("1");
            Debug.Log(WeapenStyle);
            hitbox.SetActive(true);//启用武器
        }
        if (WeapenStyle == 2)// 弓箭类武器的调用 
        {
            Debug.Log("2");
            bow.Bow.GetComponent<SpriteRenderer>().color = Color.white;
        }
        isAttack = true;//bool值
        StartCoroutine(TimeRecord());
    }

    IEnumerator TimeRecord()//携程记录攻击时间
    {
        yield return new WaitForSeconds(0.22f);
        StopNormalAttack();
    }

    void StopNormalAttack()//停止攻击
    {
        isAttack = false;
        animator.SetBool("IsAttack", false);//攻击动画的转向
        hitbox.SetActive(false);//启用武器      
    }
    void LayerChange()
    {
        this.gameObject.layer = 6;
    }
    void Climb()//爬梯子
    {
        if (isLadder && isClimbing)
            rb.gravityScale = 0;
        if(isLadder)
        {
            if (Input.GetAxisRaw("Vertical") != 0)
            {
                isClimbing = true;
            }
            rb.velocity = new Vector2(rb.velocity.x, Input.GetAxisRaw("Vertical")*4f);
        }
    }
    void ClimbCheck()//检查在梯子上时的一系列动作
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)//玩家的受击
    {
        if (collision.gameObject.tag == "enemy" || collision.gameObject.tag == "Bullet")// 角色受击！！！！！
        {
            beattack = true;
            if (collision.gameObject.transform.position.x - this.gameObject.transform.position.x >= 0)
                direction_e_p = -1;
            if (collision.gameObject.transform.position.x - this.gameObject.transform.position.x < 0)
                direction_e_p = 1;
            //GetComponent<SpriteRenderer>().color = Color.red;
            StartCoroutine(BeAttackedInvincibleTime());//启用受击闪烁的携程
            this.GetComponentInChildren<HpControl>().hp -= 25; //血量减少
            if (collision.gameObject.tag == "Bullet")
            {
                Instantiate(attackEffect, transform.position, Quaternion.identity);//生成攻击特效                
                Destroy(collision.gameObject);//销毁子弹
            }
            rb.AddForce(new Vector2(direction_e_p * Force, 2f), ForceMode2D.Impulse);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ladder")
        {
            isLadder = true;
        }
        else
        {
            isLadder = false;
            isClimbing = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")
        {
            isLadder = false;
            isClimbing = false;
        }
    }
}

