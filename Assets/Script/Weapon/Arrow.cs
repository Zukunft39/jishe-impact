using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{ // Start is called before the first frame update
    Rigidbody2D rb;
    Vector2 SpeedDic;
    void Start()
    {
        SpeedDic = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        Destroy(this.gameObject,3f);
    }
    private void Update()
    {
        if (rb.velocity.x >= 0)
            SpeedDic.x = Mathf.Abs(SpeedDic.x);
        if(rb.velocity.x <= 0)
            SpeedDic.x = -Mathf.Abs(SpeedDic.x);
        rb.transform.localScale = SpeedDic;


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="enemy")
            Destroy(this.gameObject);
    }
    // Update is called once per frame
}
