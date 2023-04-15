using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BOW : MonoBehaviour
{
    public Player player;
    public GameObject arrow;
    public Animator Bowani;
    public GameObject Bow;
    public float ArrowSpeed = 10f;
    bool ispulling=false;
    float Skillspeed=-30f;
    Transform Skill_Lastposition;
    Vector2 SkillDirection;
    float SkillLastTime = -10f,SkillCD=1f,SkillTime=0.1f;//技能需要用的
    public GameObject SkillArrow;    
    public int Bullet_tiao=5;
    private float Bullet_Tiao_temp=5;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Bullet_tiao < 6&&Bullet_Tiao_temp<6)
        {
            Bullet_Tiao_temp += 0.2f * Time.deltaTime;
            Bullet_tiao = (int)Bullet_Tiao_temp;
        }
        if (player.WeapenStyle == 2)
        {
            if(!player.isSkill)
                if (Input.GetAxisRaw("Horizontal")!=0)
                SkillDirection = new Vector2(Input.GetAxisRaw("Horizontal"),-0.5f);
            if (Input.GetKeyDown(KeyCode.J)&&Bullet_tiao>0)
            {                           
                if(!player.isSkill)
                     ispulling = true;
            }
            if(Input.GetKeyDown(KeyCode.O) && player.isGround)
            {               
                Debug.Log(SkillDirection);
                if (SkillLastTime + SkillCD <= Time.time)
                {
                    player.isSkill = true;
                    Skill_Lastposition = player.rb.transform;
                    SkillLastTime = Time.time; 
                    if(ispulling)
                        Bow.GetComponent<SpriteRenderer>().color = Color.white;
                }

            }
            if (ispulling)
            {
                BowPull();
                if (Input.GetKeyUp(KeyCode.J) || !Input.GetKey(KeyCode.J))
                {
                    BowShoot();
                    ArrowSpeed = 10f;
                }
            }
            if (player.isSkill)
            {
                Bowskill_1();
            }
        }
    }
    void BowPull()
    {
        Bow.SetActive(true);
        Bowani.enabled = true;
        if (Input.GetKey(KeyCode.J) && ArrowSpeed <= 50)
        {
            ArrowSpeed += Time.deltaTime * 20;
        }
    }
    void BowShoot()
    {
        Instantiate(arrow, new Vector2(player.transform.position.x + player.Playertran.normalized.x, player.transform.position.y-0.3f), Quaternion.identity).GetComponent<Rigidbody2D>().velocity += new Vector2(ArrowSpeed * player.Playertran.normalized.x, 0.1f);
        Bowani.enabled = false;
        Bow.GetComponent<SpriteRenderer>().color = Color.clear;
        ispulling= false;
        Bullet_tiao--;
        Bullet_Tiao_temp--;
    }
   void Bowskill_1()
    {
        if (SkillTime >= 0)
        {
            player.rb.velocity = SkillDirection * Skillspeed;
            SkillTime -= Time.deltaTime;
            ShadowPool.instance.GetFormPool();
        }
        if(SkillTime < 0)
        {
            player.isSkill = false;
            SkillTime = 0.1f;
            BowSkill_2();
        }        
    }
    void BowSkill_2()
    {
        Instantiate(SkillArrow, Bow.transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity+=SkillDirection*10f;
    }
}