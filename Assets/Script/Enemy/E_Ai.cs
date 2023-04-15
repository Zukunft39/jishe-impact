using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class E_Ai : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rb_e;
    public Rigidbody2D player0;
    public bool attack = false;
    float speed1 = 1.5f;
    private float tick1 = 2f;
    int temp = 1;
    bool beattack=false;
    //以下为角色动画
    public Animator animator;//角色的状态机
    public float HP;

    void Start()
    {
        rb_e = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb_e.velocity += Vector2.down * 9.8f * Time.deltaTime;
        if(!beattack)
        { if (attack)
        {
            rb_e.velocity = new Vector2((new Vector2((player0.position.x - rb_e.position.x),0).normalized.x)* speed1,rb_e.velocity.y);
            Vector3 enemy_scale = rb_e.transform.localScale;
            float ttemp = player0.position.x - rb_e.position.x;
            if (ttemp > 0)
            {
                enemy_scale.x = Mathf.Abs(enemy_scale.x);
                temp =1;
            }
            if (ttemp < 0)
            {
                enemy_scale.x = Mathf.Abs(enemy_scale.x) * -1;
                temp = 0;
            }
            rb_e.transform.localScale = enemy_scale;
            tick1 = 2f;
        }
            if (!attack)
            {
                if (tick1 >= 0)
                {
                    tick1 -= Time.deltaTime;
                    if (temp == 1)
                    {
                        rb_e.velocity = new Vector2(1f, rb_e.velocity.y);
                    }
                    if (temp == 0)
                        rb_e.velocity = new Vector2(-1f, rb_e.velocity.y);

                }
                else

                {
                    tick1 = 2f;
                    temp = (temp + 1) % 2;
                    Vector3 enemyscale = rb_e.transform.localScale;
                    if (temp == 1)
                    {
                        enemyscale = new Vector3(Mathf.Abs(enemyscale.x), enemyscale.y, enemyscale.z);
                    }
                    if (temp == 0)
                    {
                        enemyscale = new Vector3(Mathf.Abs(enemyscale.x) * -1, enemyscale.y, enemyscale.z);
                    }
                    rb_e.transform.localScale = enemyscale;
                }
            }
        }



        }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Hitbox")//受击
        {
            beattack = true;
            if(collision.transform.position.x>this.transform.position.x)
            {
                rb_e.AddForce(new Vector2(-2, 0.4f) , ForceMode2D.Impulse);
            }
            else
            {
                rb_e.AddForce(new Vector2(2, 0.4f) , ForceMode2D.Impulse);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Ground")
        {
            beattack=false;
        }
    }

    void Attack()
    {

    }
    public void Damage(float n)
    {
        HP -= n;
        if (HP <= 0)
            Destroy(this.gameObject);
    }
}

