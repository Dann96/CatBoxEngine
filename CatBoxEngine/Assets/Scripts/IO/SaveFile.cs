using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveFile
{
    public Dictionary<string, string> gamePersistenceValues = new Dictionary<string, string>();

    public int[] inventory = new int[10];
}
