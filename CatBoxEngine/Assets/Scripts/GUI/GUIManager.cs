using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GUIManager : PersistentSingleton<GUIManager>
{
    [SerializeField] protected CanvasGroup m_DebugLoggingPanel;
    [SerializeField] protected CanvasGroup m_DebugMenuPanel;

    private Canvas m_Canvas;
    private EventSystem m_EventSystem;

    private bool m_IsTypingText = false;

    public EventSystem eventSystem
    {
        get { return m_EventSystem; }
    }

    public bool IsTypingText
    {
        get { return m_IsTypingText; }
    }

    new void Awake()
    {
        base.Awake();
        m_Canvas = GetComponent<Canvas>();
        m_EventSystem = GetComponent<EventSystem>();

        ToggleGroup(m_DebugLoggingPanel, GameManager.instance.m_DebugMode);
        ToggleGroup(m_DebugMenuPanel, GameManager.instance.m_DebugMode);
    }

    private void Update()
    {
        
    }

    public void ToggleGroup(CanvasGroup group, bool active)
    {
        group.gameObject.SetActive(active);
        group.interactable = active;
    }


    //////////////////////////Utility Methods//////////////////////////////

    public void ShowGUIMessage(string message, DialogueFlavor flavor, Vector3 position, Vector2 anchorMin, Vector2 anchorMax)
    {

    }

    public void DisplayGUIText(Text textObject, string message, DialogueFlavor flavor)
    {

    }
   
}
