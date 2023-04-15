using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowSprite : MonoBehaviour
{
    private Transform player;//定位玩家的位置

    private SpriteRenderer thisSprite;//当前位置Sprite
    private SpriteRenderer playerSprite;//玩家的Sprite

    private Color color;//颜色属性

    [Header("时间控制参数")]
    public float activeTime;//显示时间
    public float activeStart;//开始的时间点

    [Header("不透明度控制")]
    private float alpha;//随着显示时间均匀变化的变量
    public float alphaSet;//初始不透明度
    public float alphaMultiplier;//变化的速率

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        thisSprite = GetComponent<SpriteRenderer>();
        playerSprite = player.GetComponent<SpriteRenderer>();

        alpha = alphaSet;
        thisSprite.sprite = playerSprite.sprite;

        transform.position = player.position;
        transform.rotation = player.rotation;
        transform.localScale = player.localScale;

        activeStart = Time.time;
    }

    void Update()
    {
        alpha *= alphaMultiplier;

        color = new Color(0.2157f,0.65f,1,alpha);//变色
        thisSprite.color = color;//赋值

        if(Time.time >= activeStart + activeTime)
        {
            //返回对象池
            ShadowPool.instance.ReturnPool(this.gameObject);//调用对象池
        }
    }
}
