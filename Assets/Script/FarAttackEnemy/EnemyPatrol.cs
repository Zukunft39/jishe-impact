using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    private Rigidbody2D rb_FarAttackEnemy;
    //以下为仇恨范围部分
    [SerializeField] private float attackRange;//仇恨范围   
    private Transform target;//玩家位置
    //以下为巡逻部分
    public Transform wayPoint01;//巡逻固定点
    public Transform wayPoint02;
    private Transform wayPointTarget;//当前巡逻目标
    [SerializeField] private float speed;//巡逻速度
    //以下为发射子弹部分
    public GameObject bullet;
    public Transform shotPlace;
    //以下为计时器
    public float timer;
    public float HP;

    void Start()
    {
        rb_FarAttackEnemy = GetComponent<Rigidbody2D>();
        wayPointTarget = wayPoint01;//初始化朝向
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();//找到玩家的位置

    }
    void Update()
    {
        rb_FarAttackEnemy.velocity += Vector2.down * 9.8f * Time.deltaTime;//施加重力
        timer += Time.deltaTime;//计时

        if (Vector2.Distance(target.position, transform.position) > attackRange)
        {
            Patrol();//巡逻
        }
        else
        {
            Shot();//射击
        }
    }

    void Patrol()
    {
        transform.position = Vector2.MoveTowards(transform.position, wayPointTarget.position, speed * Time.deltaTime);//朝着目标点巡逻移动

        if (Vector2.Distance(transform.position, wayPoint01.position) < 0.5f)//判断距离
        {
            Vector3 localTemp = transform.localScale;
            localTemp.x *= -1;
            transform.localScale = localTemp;//图片翻转
            wayPointTarget = wayPoint02;//切换巡逻对象
        }
        if (Vector2.Distance(transform.position, wayPoint02.position) < 0.5f)
        {
            Vector3 localTemp = transform.localScale;
            localTemp.x *= -1;
            transform.localScale = localTemp;
            wayPointTarget = wayPoint01;
        }
    }
    void Shot()
    {
        if(timer % 1.5 > 1&&timer %1.5 <1.01) 
        {
            Debug.Log("发射子弹");
            timer = 0;
            Instantiate(bullet, shotPlace.position, transform.rotation);//生成子弹（锁定帧数120的情况下为发射一个子弹）
        }       
    }
    private void OnCollisionEnter2D(Collision2D collision)//玩家的受击
    {
        if(collision.gameObject.tag == "Hitbox")
        {
            this.GetComponentInChildren<HpControl>().hp -= 25; //血量减少
        }
    }
}
