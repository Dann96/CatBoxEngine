using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugLoggingPanel : MonoBehaviour
{
    public Text m_LoggingText;

    private void Awake()
    {
        Application.logMessageReceived += LogText;
    }

    public void LogText(string entry, string trace, LogType type)
    {
        switch (type)
        {
            case LogType.Log:
                m_LoggingText.text += "\n-" + entry;
                break;
        }
    }
}