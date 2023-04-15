using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPool : MonoBehaviour
{
    public static ShadowPool instance;//函数外均可调用

    public GameObject shadowPrefab;//声明游戏内物体,对象池中的预制体

    public int shadowCount;//多少个残影

    private Queue<GameObject> availableObjects = new Queue<GameObject>();
    void Awake()
    {
        instance = this;

        //初始化对象池
        FillPool();
    }
    void Update()
    {
        
    }
    public void FillPool()//填充对象池
    {
        for(int i = 0; i < shadowCount; i++)
        {
            var newShadow = Instantiate(shadowPrefab);//临时变量，让每一个生成的预制体都在ShadowPool作为子物体
            newShadow.transform.SetParent(transform);

            //取消启用，返回对象池
            ReturnPool(newShadow);
        }
    }

    public void ReturnPool(GameObject gameObject)
    {
        gameObject.SetActive(false);//取消启用GameObject

        availableObjects.Enqueue(gameObject);//向队列尾添加元素
    }

    public GameObject GetFormPool() 
    {
        if (availableObjects.Count == 0)//保险，如果对象池中Count为0就再次进行填充
        {
            FillPool();
        }

        var outShadow = availableObjects.Dequeue();//从对象池里面选出一个物体并且让其显示出来
        outShadow.SetActive(true);//激活

        return outShadow;//返回临时变量
    }
}
