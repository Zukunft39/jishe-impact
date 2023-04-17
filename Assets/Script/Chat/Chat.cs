using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class Chat : MonoBehaviour
{
    public int process=-1;
    public Node[] nodeList;
    public Sprite npcAvatar;
    public Sprite playerAvatar;

    ChatManager chatMgr;
    public Chat(SingleChat singleChat, ChatManager mgr, Sprite _playerAvatar, Sprite _npcAvatar)
    {
        nodeList = singleChat.chat;
        chatMgr = mgr;
        playerAvatar = _playerAvatar;
        npcAvatar = _npcAvatar;
        
    }
    //return true when this chat is over
    public bool ChatContinue()
    {
        if (process + 1 >= nodeList.Length)
        {
            chatMgr.playerDialog.Deactivate();
            chatMgr.npcDialog.Deactivate();
            Debug.Log("ChatOver");
            return true;
        }
        process++;
        
        switch (nodeList[process].type)
        {
            case NodeType.Dialog:
                if (nodeList[process].isPlayer)
                {
                    chatMgr.playerDialog.Activate();
                    chatMgr.npcDialog.Deactivate();
                    if (nodeList[process].avatar != null)
                        chatMgr.playerDialog.SetDialog(nodeList[process].name, nodeList[process].content, nodeList[process].avatar);
                    else
                        chatMgr.playerDialog.SetDialog(nodeList[process].name, nodeList[process].content,playerAvatar);
                }
                else
                {
                    chatMgr.npcDialog.Activate();
                    chatMgr.playerDialog.Deactivate();
                    if (nodeList[process].avatar != null)
                        chatMgr.npcDialog.SetDialog(nodeList[process].name, nodeList[process].content, nodeList[process].avatar);
                    else
                        chatMgr.npcDialog.SetDialog(nodeList[process].name, nodeList[process].content, npcAvatar);
                }
                break;
            case NodeType.Action:
                nodeList[process].action.Invoke();
                ChatContinue();
                break;
        }
        return false;
    }
}

public enum NodeType
{
    Dialog,Action
}
[System.Serializable]
public struct Node 
{
    public NodeType type;
    public string name;
    public string content;
    public bool isPlayer;
    public UnityEvent action;
    public Sprite avatar;
}
[System.Serializable]
public struct SingleChat
{
    public string progressDescription;
    public Node[] chat;
}
[System.Serializable]
public struct NPCChats
{
    public string npcName;
    public SingleChat[] chats;
}
[System.Serializable]
public struct NPCsStory
{
    public NPCChats[] npc;
}


