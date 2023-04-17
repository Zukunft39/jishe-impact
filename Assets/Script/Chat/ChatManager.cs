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

    private Chat currentChat;

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
    public void StartChat(NPC npc)
    {
        if (isChatting) return;
        SingleChat chatContent = NPCStory.Instance.storyContent.npc[(int)NPCID.test].chats[npc.storyProgress];
        currentChat = new(chatContent, this, null, null);
        playerDialog.transform.position = playerCon.groundCheck.position;
        npcDialog.transform.position = npc.transform.position;
        isChatting = true;
        playerCon.isChatting = true;
        if (currentChat.ChatContinue()) QuitChat();
    }

    private void Update()
    {
        if (isChatting)
        {
            if (Input.GetKeyDown(KeyCode.Space)|| Input.GetKeyDown(KeyCode.Return)||Input.GetKeyDown(KeyCode.Joystick1Button1))
            {
                if (currentChat.ChatContinue()) QuitChat();
            }
        }
    }

    public void QuitChat()
    {
        isChatting = false;
        playerCon.isChatting =false;
    }
}
