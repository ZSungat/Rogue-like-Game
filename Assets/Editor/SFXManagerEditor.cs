using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SFXManager))]
public class SFXManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SFXManager manager = (SFXManager)target;

        // Add a button to skip music
        if (GUILayout.Button("Skip Music"))
        {
            manager.SkipMusic();
        }
    }

}
