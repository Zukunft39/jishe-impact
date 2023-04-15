using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour
{
    bool isClosed;
    public GameObject bag;

    public void OpenCloseBag()
    {
        if (isClosed == true)//如果背包处于关闭状态
        {
            bag.SetActive(true);
            isClosed = false;
        }
        else if(isClosed == false)
        {
            bag.SetActive(false);
            isClosed = true;
        }
    }
}
