using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStory : MonoBehaviour
{
    public static NPCStory Instance { get; private set; }
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
    }
    public NPCsStory storyContent;
}
