using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PersistentData : ISerializationCallbackReceiver
{
    public int currentIndex = 0;

    public GameSettings gameSettings = new GameSettings();

    public SaveFile[] saveFiles = new SaveFile[3];

    public void OnBeforeSerialize(){}

    public void OnAfterDeserialize(){}

    public SaveFile CurrentFile
    {
        get { return saveFiles[currentIndex]; }
    }
}
