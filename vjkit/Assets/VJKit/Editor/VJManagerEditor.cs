using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(VJManager))]
public class VJManagerEditor : Editor 
{
    public VJManagerEditor()
    {
    }

    public override void OnInspectorGUI()
    {
        GUI.changed = false;

        base.OnInspectorGUI();

		VJManager manager = target as VJManager;
		bool b = EditorGUILayout.Toggle("Use Microphone", manager.isMicSource);
		
		if(b != manager.isMicSource) {
			manager.ToogleMicSource(b);
		}

        if (GUI.changed) {
            EditorUtility.SetDirty(target);
        }
    }
}
