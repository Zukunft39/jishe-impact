using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletControl : MonoBehaviour
{
    public Image Bullet_Image;
    private float Maxnum = 6f;
    private float Nownum=6f;
    public BOW bOW;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Nownum = bOW.Bullet_tiao;
        Bullet_Image.fillAmount = Nownum / Maxnum;
    }
}
