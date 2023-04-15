using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraFollow : MonoBehaviour
{
    public float followSpeed = 5.0f;//获取相机跟随速度
    public Transform target;//获取目标位置
    public float way1;
    public float way2;
    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x+way1, target.position.y+way2, -10); 
        transform.position = Vector3.Slerp(transform.position, newPos, followSpeed * Time.deltaTime);//让相机的位置跟其保持一致
    }
}
