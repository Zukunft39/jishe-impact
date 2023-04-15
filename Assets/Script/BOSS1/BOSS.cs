using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BOSS : MonoBehaviour
{
    Rigidbody2D Boss;
    RaycastHit2D[] Close;
    RaycastHit2D[] Far;
    bool isFar, isNear;
    Vector2 BossDic;
    Transform player;
    float WalkSpeed = 3f;
    Vector2 Temp;//用来改变boss方向;
    bool isAttack = true, isDash = false, WantAttack = true,Dash_Skill_Attack=false,Skill_1_Attack=false,RockAttack=false;
    float AttackTime, MoveTick=2f;
    float B_Dashspeed = 20f, DasTime = 0.3f;
    public GameObject SwordHitBox, SwordHitBox_2;//攻击碰撞箱
    int SkillStyle;
    float ReallyTick = 2;//标准计时器的秒数
    float AttackTick;
    public GameObject Rock;
    Vector2 PlayerPosition;
    bool F_Skill3_GC,F_Skill3_GD,F_Skill3_AT;
    Animator anim;
    float HP;

    // Start is called before the first frame update
    void Start()
    {
        anim=GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;//找玩家位置
        Boss = GetComponent<Rigidbody2D>();       
        BossDicJug();
        BossDicChange();
        if (Boss.velocity.x!=0)
        {
            anim.SetBool("isMove", true);
        }
        else
        {
            anim.SetBool("isMove", false);
        }
        if (!isAttack)
        {
            BossMove();
            MoveTick -= Time.deltaTime;
            if(MoveTick<=0)
            {
                isAttack = true;
                MoveTick = Random.Range(1, 2);                
            }
        }
        else
        {
            if (WantAttack)
            {
                SkillStyle = Random.Range(1, 20);
                WantAttack = false;
                RaycastFar();
                RaycastClose();             
                if (SkillStyle <= 6)//每个方式的前置条件
                {
                    AttackTick = 1f;
                    Skill_1_Attack = true;
                }
                else if (SkillStyle >= 7 && SkillStyle <= 10)
                    AttackTick = 2f;
                else if (SkillStyle >= 10&&SkillStyle<=13)
                {
                    AttackTick = 2f;
                    isDash = true;
                    Dash_Skill_Attack = true;
                }            
                else if(SkillStyle >= 14&&SkillStyle <= 17)
                {
                    RockAttack = true;
                    AttackTick = 2f;
                }
                else if(SkillStyle>=18&&SkillStyle<=20)
                {                 
                    AttackTick = 4f;
                    F_Skill3_GC = false;
                    F_Skill3_GD = false;
                    Debug.Log("0909");
                    F_Skill3_AT = true;
                }
            }
            if (isNear)
            {
                if (SkillStyle <= 6)
                {
                    BossSkillClose_1();
                    Debug.Log("C1");
                }
                if (SkillStyle >= 7 && SkillStyle <= 10)
                {
                    BossSkillClose_2();
                    Debug.Log("C2");

                }
            }
            if (isFar)
            {
                if (SkillStyle >= 10 && SkillStyle <= 13)
                {
                    BossSkillFar_1();
                    Debug.Log("F1");
                }
                if (SkillStyle >= 14 && SkillStyle <= 17)
                {
                    BossSkillFar_2();
                    Debug.Log("F2");
                }
                if(SkillStyle>=18&&SkillStyle<=20)
                {
                    BossSkillFar_3();
                    Debug.Log("F3");
                }
            }
         AttackTick-=Time.deltaTime;
            if(AttackTick < 0)//攻击时间计时0.0
            {
                isAttack = false;
                WantAttack = true;
                Debug.Log("Finish");
                GetComponent<BoxCollider2D>().enabled = false;
                WalkSpeed = 3f;
                F_Skill3_GD = false;
                F_Skill3_GC = false;
                isFar = false;
                isNear = false;
                anim.SetBool("isFall", false);
                anim.SetBool("isFarAttack_1", false);
                anim.SetBool("isFarAttack_2", false);
                anim.SetBool("isAttack_1", false);
            }
        }
    }
    void RaycastClose()//近战判断
    {
        bool isPlayer=false;
        Close = Physics2D.RaycastAll(new Vector2(transform.position.x, player.position.y), BossDic, 2f);
        foreach(var Hit in Close)
        {
            if (Hit.transform.tag == "Player")
                isPlayer = true;
        }
        if (isPlayer)
        {
            isNear = true;
            isFar = false;
        }
        else
            isNear = false;
    }
    void RaycastFar()
    {
        bool isPlayer = false;
        Far = Physics2D.RaycastAll(new Vector2(transform.position.x,player.position.y), BossDic, 10f);
        foreach (var Hit in Far)
        {
            if (Hit.transform.tag == "Player")
                isPlayer = true;
        }
        if (isPlayer)
        {
            isFar = true;
            isNear = false;
        }
        else
            isFar = false;
    }

    void BossDicJug()//得到Boss的方向
    {
        if (Boss.transform.localScale.x >0)
            BossDic = new Vector2(1, 0);
        else if(Boss.transform.localScale.x < 0)
            BossDic = new Vector2(-1, 0);
        
    }
    void BossDicChange()//Boss方向的改变判断
    {
        Temp = new Vector2(Boss.transform.localScale.x, Boss.transform.localScale.y);
        if (Boss.velocity.x > 0)
            Temp.x = Mathf.Abs(Boss.transform.localScale.x);
        if (Boss.velocity.x < 0)
            Temp.x = -Mathf.Abs(Boss.transform.localScale.x);
        Boss.transform.localScale = Temp;
    }
    void BossMove()//Boss的移动
    {
        if (player.position.x <= Boss.position.x)
            Boss.velocity = new Vector2(-WalkSpeed, Boss.velocity.y);
        else
            Boss.velocity = new Vector2(WalkSpeed, Boss.velocity.y);
    }
    void BossSkillClose_1()
    {
        Boss.velocity = new Vector2(0, 0);
        if (Skill_1_Attack)
        {
            anim.SetBool("isAttack_1", true);
            Invoke("CloseAttack_1", 0.2f);
            Invoke("CloseAttack_1_1", 0.3f);
            Skill_1_Attack = false;
        }
     
    }
    void BossSkillClose_2()
    {
        //播放旋转动画0.0?
        WalkSpeed = 7f;
        GetComponent<BoxCollider2D>().enabled = true;
        BossMove();
        Debug.Log(WalkSpeed);
    }
    void BossSkillFar_1()
    {
        if (isDash)
        {
            Debug.Log("a");
            anim.SetBool("isDash", true);
            BossDash();
           DasTime-=Time.deltaTime;
            if (DasTime <= 0)
            {
                isDash = false;
                DasTime = Random.Range(0.2f,0.3f);                           
            }
        }
        if (!isDash)
        {
            if (Dash_Skill_Attack)// 攻击一次
            {
                anim.SetBool("isDash", false);
                anim.SetBool("isFarAttack_1", true);
                Boss.velocity = new Vector2(BossDic.x/100, 0);
                Invoke("DASHATTACK",0.4f);
                Dash_Skill_Attack = false;
                anim.SetBool("isFarAttack_2", false);
            }
        }
    }
    void BossSkillFar_2()
    {
        Boss.velocity = new Vector2(0, 0);
        if (RockAttack)
        {
            anim.SetBool("isFarAttack_2", true);
            for (int i = 0; i < 3; i++)
            {
                Instantiate(Rock, new Vector2(transform.position.x + BossDic.x - 0.1f, transform.position.y - 1f), Quaternion.identity).GetComponent<Rigidbody2D>().velocity += new Vector2(i * 3f * BossDic.x, (10f - i));
            }
            RockAttack = false;
        }
        Invoke("FarAttack_2", 0.1f);
    }
    void   BossSkillFar_3()
    {
        if (F_Skill3_AT)
        {          
            StartCoroutine(BossSkillFar_3_());
            F_Skill3_AT = false;
        }
        if (F_Skill3_GC)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector2(PlayerPosition.x, PlayerPosition.y + 6f), 0.2f) ;
            Debug.Log("F3_1");
        }
        if(F_Skill3_GD)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector2(transform.position.x,0f), 0.4f);
            Boss.velocity=new Vector2 (0,Boss.velocity.y);
            Debug.Log("F3_2");
        }

    }
    void BossDash()
    {
        Boss.velocity = new Vector2(B_Dashspeed * BossDic.x, 0);
        Debug.Log("isDashing");
    }
    IEnumerator BossSkillFar_3_()
    {
        yield return new WaitForSeconds(0.2f);
        Boss.velocity = Boss.velocity / 5;
        PlayerPosition = new Vector2((player.position.x + Random.Range(0.8f, 1.6f) * (player.GetComponent<Rigidbody2D>().velocity.x)), player.position.y);
        anim.SetBool("isJump", true);
        yield return new WaitForSeconds(0.02f);
        anim.SetBool("isJump", false);
        yield return new WaitForSeconds(1f);
        F_Skill3_GC = true;
        anim.SetBool("isFall", true);
        yield return new WaitForSeconds(0.8f);    
        F_Skill3_GC = false;
        F_Skill3_GD = true;
        PlayerPosition = player.position;
        Debug.Log(F_Skill3_GC);
        yield return new WaitForSeconds(0.2f);
        for(int i=1;i<=2;i++)
        {
            Instantiate(Rock, new Vector2(transform.position.x + BossDic.x - 0.1f, transform.position.y - 1f), Quaternion.identity).GetComponent<Rigidbody2D>().velocity += new Vector2(i * 3f * BossDic.x, (10f - i));
            Instantiate(Rock, new Vector2(transform.position.x - BossDic.x + 0.1f, transform.position.y - 1f), Quaternion.identity).GetComponent<Rigidbody2D>().velocity += new Vector2(i * -3f * BossDic.x, (10f - i));
            Instantiate(Rock, new Vector2(transform.position.x - BossDic.x + 0.1f, transform.position.y - 1f), Quaternion.identity).GetComponent<Rigidbody2D>().velocity += new Vector2(i * -1f * BossDic.x, (8f - i));
            Instantiate(Rock, new Vector2(transform.position.x + BossDic.x - 0.1f, transform.position.y - 1f), Quaternion.identity).GetComponent<Rigidbody2D>().velocity += new Vector2(i * 1f * BossDic.x, (8f - i));
        }
        Debug.Log("down");
    }
    void TickRecord(float Tick, bool isdoing)
    {
        Tick -= Time.deltaTime;
        if (Tick <= 0)
        {
            isdoing = false;
         
        }
    }
    void TickRecord_T(float Tick, bool isdoing)
    {
        Tick -= Time.deltaTime;
        if (Tick <= 0)
        {
            isdoing = true;

        }
    }
    void DASHATTACK()
    {
        Destroy(Instantiate(SwordHitBox_2, new Vector2(transform.position.x + BossDic.x, transform.position.y), Quaternion.identity), 0.4f);
    }
    void FarAttack_2()
    {
        anim.SetBool("isFarAttack_2", false);
    }
    void CloseAttack_1()
    {
        anim.SetBool("isAttack_1", false);
   
    }
    void CloseAttack_1_1()
    {
        Destroy(Instantiate(SwordHitBox, new Vector2(transform.position.x + BossDic.x, transform.position.y), Quaternion.identity), 0.2f);
    }
    public void Damage(float n)
    {
        HP -= n;
        if (HP <= 0)
            Destroy(this.gameObject);
    }
}
