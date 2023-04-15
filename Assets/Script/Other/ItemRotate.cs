using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotate : MonoBehaviour
{
    public float speed;//Ðý×ªËÙ¶È
    void Update()
    {
        transform.Rotate(Vector2.up * speed);
    }
}
