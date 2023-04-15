using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;

public class Pickup : MonoBehaviour
{
    private Inventory inventory;
    public GameObject itemButton;


    private void Start()
    {
        inventory= GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();//找到玩家的标签并创造库存
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))//对比标签
        {
            for(int i = 0; i < inventory.slots.Length; i++)//遍历槽
            {
                if (inventory.isFull[i] == false)
                {
                     Destroy(gameObject);//拾取后销毁地图中的道具
                     inventory.isFull[i] = true;//拾取道具后判定为已拾取
                     Instantiate(itemButton, inventory.slots[i].transform, false);//生成按钮在图片位置
                     break;
                }              
            }
        }
    }
}
