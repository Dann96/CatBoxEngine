using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugMenuPanel : MonoBehaviour
{
    public InputField m_SceneField;

    public void LoadDebugLevel()
    {
        LoadLevel(m_SceneField.text);
    }

    public void LoadLevel(string sceneName)
    {
        GameManager.instance.LoadLevel(sceneName);
    }
}