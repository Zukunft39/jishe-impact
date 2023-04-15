using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class E_AI_Range : MonoBehaviour
{
    public E_Ai E_Ai;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
    
        if (collision.tag == "Player")
        {
            E_Ai.player0 = collision.GetComponent<Rigidbody2D>();
            E_Ai.attack = true;
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            E_Ai.attack = false;
        }

    }
}
