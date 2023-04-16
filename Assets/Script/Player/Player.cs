using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public bool isSkill;//�Ƿ��ڼ�����
    public BOW bow;//���Ľű�
    public int WeapenStyle = 2;
    public static Player player;
    public Rigidbody2D rb;
    public float speed = 5f;
    public float h = 5;
    public float g = 10f;
    float v;
    Vector2 inputpos;
    public static Vector2 transfor;
    public Vector3 Playertran;//��ҵĳ���
    //����Ϊ��̴���
    bool isdash = false;//�Ƿ���
    float dashtime = 0.2f;
    float dashtimeleft, dashCD = 1f, dashLast = -10f;
    float dashspeed = 50f;
    int Dashdirection_X;
    float Dashdirection_Y;//���������
    float Force = 5f;
    bool Wantdash = false;
    bool isAttack = false,isLadder=false,isClimbing=false;
    //����Ϊ��Ծ���
    [Range(1, 10)]
    private float jumpSpeed = 8f;
    private bool moveJump;//�ж��Ƿ�����Ծ
    public bool isGround;//�ж��Ƿ��ڵ�����
    public Transform groundCheck;//������
    public LayerMask ground;
    //����Ϊ��Ծ�Ż�
    public float fallMultiplier = 1000f;//����������
    public float lowJumpMultiplier = 600f;//С��������
    public float fallMultiplierElse = 3f;//��������
    //����Ϊ��������ܵ�ʵ��
    public int jumpCount = 2;//��Ծ����
    private bool isJump;//��ʾ��Ծ״̬
    //����Ϊ�ӵ���Ч
    public GameObject destroyEffect;//�ӵ����ٵ���Ч
    public GameObject attackEffect;//��������ҵ���Ч
    //����Ϊ�ܻ����޵�ʱ��
    public float flashes;//��˸����
    public float duration;//��˸����
    private SpriteRenderer sr;//��ɫ
    //����ΪԶ�̵��˹��������ʱ����ܻ��󳷵Ĺ���ʵ��
    public Transform player_0;//��ȡ��ҵ�λ�����
    public Transform farAttackEnemy;//��ȡ���˵�λ�����
    private float BeattackTime = 0.2f;//�ܻ�ʱ��
    bool beattack = false;
    int direction_e_p = 0;
    //����Ϊ��ɫ����
    public Animator animator;//��ɫ��״̬��
    //����Ϊ��ɫ����
    public GameObject hitbox;//ͽ�ֽ�ս
    //����Ϊ������ʱ��
    public float attackTimer;//����ʱ��

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
        sr = gameObject.GetComponent<SpriteRenderer>();//��ɫ���
        hitbox.SetActive(false);//��������
    }
    void Update()
    {
        transfor = this.gameObject.transform.position;
        if (!beattack && !isSkill)
        {
            if (!isdash)//����ж���������д0.0
            {
                PlayerJumpByTwice();
                MoveObject();
                Climb();
                if (Input.GetKeyDown(KeyCode.J))//����
                {
                    isAttack = true;
                    NormalAttack();//���ڹ���

                }
                else
                {
                    //isAttack = false;
                    //animator.SetBool("IsAttack", false);//����������ת��
                    //hitbox.SetActive(false);//��������
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
    private void FixedUpdate()//�̶�Ϊÿ��50�μ��Ĺ̶��������
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);//������
    }
    public void MoveObject()//�����ҵĳ���Ļ����ƶ�
    {
        inputpos = rb.velocity;
        inputpos.x = Input.GetAxisRaw("Horizontal") * speed;
        animator.SetFloat("SpeedX", Mathf.Abs(inputpos.x));//���߶�����ת��
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
    void Dash()//�����������
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
            ShadowPool.instance.GetFormPool();//���ö����
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
    void Rush()//���
    {
        if (isdash)
            if (dashtimeleft >= 0)
            //transform.Translate(transform.right * Time.deltaTime * dashtime*Dashdirection*dashspeed);
            {
                this.gameObject.layer = 9;
                rb.velocity = dashspeed * new Vector2(Dashdirection_X, Dashdirection_Y);
            }
    }
    void Dashready()//���׼��
    {
        isdash = true;
        dashtimeleft = dashtime;
        dashLast = Time.time;
    }
    void PlayerJumpByTwice()//������
    {
        moveJump = Input.GetButtonDown("Jump");
        animator.SetFloat("SpeedY", inputpos.y);//��Ծ������ת��
        animator.SetBool("IsJump", true);//��Ծ������ת��
        JumpDetectionByTwice();
        //���Ƿֽ��ߣ�����Ϊ�Ż���Ծ�ָ�����
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
    void JumpDetectionByTwice()//���������
    {
        if (moveJump && jumpCount > 0)
        {
            isJump = true;
        }
        if (isGround)//�ж��Ƿ��ڵ���
        {
            jumpCount = (int)2f;//��������Ϊ2
            animator.SetBool("IsJump", false);//��Ծ������ת��
        }
        if (isJump)
        {
            jumpCount--;//�����е����⣬��һ����Ծʱ�޷���⵽��Ծ��������ʱ���ܼ�⵽�����Ҫʹ�ö����������Ļ�ֱ����������ϵ��ö����������Ϳ�����
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            jumpCount--;
            isJump = false;
        }
    }
    IEnumerator BeAttackedInvincibleTime()//�ܵ���������޵�ʱ��
    {
        Physics2D.IgnoreLayerCollision(6, 7, true);//�������ײʱ
        Physics2D.IgnoreLayerCollision(6, 10, true);//���ӵ���ײʱ
        sr.color = Color.red;
        yield return new WaitForSeconds(duration / flashes);
        for (int i = 0; i < flashes; i++)
        {
            sr.color = new Color(255 / 255, 255 / 255, 255 / 255, 0.6f);//͸���ȱ仯
            yield return new WaitForSeconds(duration / flashes);
            sr.color = Color.white;
            yield return new WaitForSeconds(duration / flashes);
        }
        Physics2D.IgnoreLayerCollision(6, 7, false);
        Physics2D.IgnoreLayerCollision(6, 10, false);
    }

    private void NormalAttack()//��ͨ����
    {
        animator.SetBool("IsAttack", true);//����������ת��
        if (WeapenStyle == 1)
        {
            Debug.Log("1");
            Debug.Log(WeapenStyle);
            hitbox.SetActive(true);//��������
        }
        if (WeapenStyle == 2)// �����������ĵ��� 
        {
            Debug.Log("2");
            bow.Bow.GetComponent<SpriteRenderer>().color = Color.white;
        }
        isAttack = true;//boolֵ
        StartCoroutine(TimeRecord());
    }

    IEnumerator TimeRecord()//Я�̼�¼����ʱ��
    {
        yield return new WaitForSeconds(0.22f);
        StopNormalAttack();
    }

    void StopNormalAttack()//ֹͣ����
    {
        isAttack = false;
        animator.SetBool("IsAttack", false);//����������ת��
        hitbox.SetActive(false);//��������      
    }
    void LayerChange()
    {
        this.gameObject.layer = 6;
    }
    void Climb()//������
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
    void ClimbCheck()//�����������ʱ��һϵ�ж���
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)//��ҵ��ܻ�
    {
        if (collision.gameObject.tag == "enemy" || collision.gameObject.tag == "Bullet")// ��ɫ�ܻ�����������
        {
            beattack = true;
            if (collision.gameObject.transform.position.x - this.gameObject.transform.position.x >= 0)
                direction_e_p = -1;
            if (collision.gameObject.transform.position.x - this.gameObject.transform.position.x < 0)
                direction_e_p = 1;
            //GetComponent<SpriteRenderer>().color = Color.red;
            StartCoroutine(BeAttackedInvincibleTime());//�����ܻ���˸��Я��
            this.GetComponentInChildren<HpControl>().hp -= 25; //Ѫ������
            if (collision.gameObject.tag == "Bullet")
            {
                Instantiate(attackEffect, transform.position, Quaternion.identity);//���ɹ�����Ч                
                Destroy(collision.gameObject);//�����ӵ�
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

