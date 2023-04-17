using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UICanvas
{
    Player,Bag,Chat
}
public class UIManager : MonoBehaviour
{
    public Canvas[] canvasList;
    public static UIManager Instance { get; private set; }
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

    public void SetCanvasActive(UICanvas id,bool isActive)
    {
        canvasList[(int)id].gameObject.SetActive(isActive);
    }
    public void SoloCanvas(UICanvas id)
    {
        for (int i = 0; i < canvasList.Length; i++) 
            canvasList[i].gameObject.SetActive(i == (int)id);
    }
}
