using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(VJAbstractDataSource))]
public class VJAbstractDataSourceEditor : Editor 
{

    public VJAbstractDataSourceEditor()
    {
    }

    public override void OnInspectorGUI()
    {
        GUI.changed = false;

		VJAbstractDataSource src = target as VJAbstractDataSource;

        base.OnInspectorGUI();
		EditorGUILayout.Space();
		Rect r = GUILayoutUtility.GetLastRect();
		r.x += 8;
		r.width -= 16;
		r.y += 20;
		r.height = 16;
		EditorGUILayout.BeginVertical();
		GUILayout.Space(72.0f);
		EditorGUI.ProgressBar(r, src.current / VJAbstractDataSource.s_prog_max, "Current:"+src.current);
		r.y += 20;
		EditorGUI.ProgressBar(r, src.previous / VJAbstractDataSource.s_prog_max, "Previous:"+src.previous);
		r.y += 20;
		EditorGUI.ProgressBar(r, src.diff / VJAbstractDataSource.s_prog_max, "Difference:"+src.diff);
		EditorGUILayout.EndVertical();

        if (GUI.changed) {
            EditorUtility.SetDirty(target);
        }
    }
}
