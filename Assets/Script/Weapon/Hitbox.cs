using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="enemy")
        {
            E_Ai enemy=collision.GetComponent<E_Ai>();
             BOSS boss=collision.GetComponent<BOSS>();
            if(enemy!=null)
            enemy.Damage(25);
            if(boss!=null)
                boss.Damage(25);
        }
    }
}
