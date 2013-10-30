using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(VJManager))]
public class VJMidiManagerEditor : Editor 
{
	public VJMidiManagerEditor()
    {
    }

    public override void OnInspectorGUI()
    {
        GUI.changed = false;

        base.OnInspectorGUI();

//		VJMidiManagerEditor manager = target as VJMidiManagerEditor;

        if (GUI.changed) {
            EditorUtility.SetDirty(target);
        }
    }
}
