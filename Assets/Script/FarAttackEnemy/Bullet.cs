using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bullet : MonoBehaviour
{
    //以下为子弹存活时间
    private float lifeTimer;//子弹存活时间
    [SerializeField] private float maxLife = 2.0f;//最大存活时间
    //以下为发射子弹部分
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed;//子弹发射速度
    //以下为子弹特效
    public GameObject destroyEffect;//子弹销毁的特效
    public GameObject attackEffect;//攻击到玩家的特效

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();//获取玩家位置
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);//射向玩家

        lifeTimer += Time.deltaTime;//计时
        if (lifeTimer >= maxLife )
        {
            Instantiate(destroyEffect,transform.position,transform.rotation);//生成特效
            Destroy(gameObject);//销毁子弹
        }
    }
    //private void OnCollsionEnter2D(Collision2D collision)
    //{
    //    if(collision.gameObject.tag == "Player")
    //    {
    //        Instantiate(attackEffect, transform.position, Quaternion.identity);//生成攻击特效
    //        Debug.Log("Attack");
    //        Destroy(gameObject);//销毁特效
    //    }
    //}
}
