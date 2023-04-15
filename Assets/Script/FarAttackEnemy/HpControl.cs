using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HpControl : MonoBehaviour
{
    public Image hpImage;//血量图片
    public Image hpEffectImage;//血量减少缓冲特效图片
    public float hp;//当前血量
    [SerializeField]private float maxHp;//最大血量
    [SerializeField]private float hurtSpeed = 0.05f;//血量减少的速度

    void Start()
    {
        hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        hpImage.fillAmount = hp / maxHp;//血量的显示
        if (hpEffectImage.fillAmount >= hpImage.fillAmount)
        {
            hpEffectImage.fillAmount -= hurtSpeed;//减少血量
        }
        else
        {
            hpEffectImage.fillAmount = hpImage.fillAmount;
        }
    }
}
