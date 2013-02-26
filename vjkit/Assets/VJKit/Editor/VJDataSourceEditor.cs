using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(VJDataSource))]
public class VJDataSourceEditor : Editor 
{

    public VJDataSourceEditor()
    {
    }

    public override void OnInspectorGUI()
    {
        GUI.changed = false;

		VJDataSource src = target as VJDataSource;

        base.OnInspectorGUI();
		EditorGUILayout.Space();
		Rect r = GUILayoutUtility.GetLastRect();
		r.x += 8;
		r.width -= 16;
		r.y += 20;
		r.height = 16;
		EditorGUILayout.BeginVertical();
		GUILayout.Space(72.0f);
		EditorGUI.ProgressBar(r, src.current / VJDataSource.s_prog_max, "Current:"+src.current);
		r.y += 20;
		EditorGUI.ProgressBar(r, src.previous / VJDataSource.s_prog_max, "Previous:"+src.previous);
		r.y += 20;
		EditorGUI.ProgressBar(r, src.diff / VJDataSource.s_prog_max, "Difference:"+src.diff);
		EditorGUILayout.EndVertical();

        if (GUI.changed) {
            EditorUtility.SetDirty(target);
        }
    }
}
