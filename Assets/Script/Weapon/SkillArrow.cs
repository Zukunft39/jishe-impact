using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SkillArrow : MonoBehaviour
{
    List<GameObject> list = new List<GameObject>();
    // Start is called before the first frame update
    Rigidbody2D rb;
    int n = 3;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (n == 0)
        {
            this.GetComponent<CapsuleCollider2D>().enabled = true;
            Destroy(this.gameObject, 0.2f);
        }

    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Ground")
        {
            n--;
        }
    }
}
