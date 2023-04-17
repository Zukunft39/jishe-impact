using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    public Text dialogName;
    public Text dialogText;
    public Image dialogAvatar;

    public void SetPos(Vector2 pos)
    {
        transform.position = pos;
    }
    public void SetDialog(string name,string text,Sprite avatar)
    {
        dialogName.text = name;
        dialogText.text = text;
        dialogAvatar.sprite = avatar;
    }
    public void Activate()
    {
        gameObject.SetActive(true);
    }
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
