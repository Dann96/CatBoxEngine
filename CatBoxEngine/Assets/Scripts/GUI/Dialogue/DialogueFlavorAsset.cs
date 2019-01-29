using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

public class DialogueFlavorAsset
{
    [MenuItem("Assets/Create/Dialogue Flavor")]
    public static void CreateDialogueFlavor()
    {
        DialogueFlavor flavor = ScriptableObject.CreateInstance<DialogueFlavor>();

        AssetDatabase.CreateAsset(flavor, "Assets/DialogueFlavors/DialogueFlavor.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = flavor;
    }
}
#endif