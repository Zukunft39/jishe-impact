using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    public static ChatManager Instance { get; private set; }
    public Player playerCon;
    public bool isChatting;
    public Dialog playerDialog;
    public Dialog npcDialog;

    protected void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        playerCon = GameObject.Find("Player").GetComponent<Player>();

    }
    public void StartChat(int npcid)
    {

    }
}
