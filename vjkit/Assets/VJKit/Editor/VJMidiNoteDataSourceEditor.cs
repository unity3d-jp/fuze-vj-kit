using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(VJMidiNoteDataSource))]
public class VJMidiNoteDataSourceEditor : VJAbstractDataSourceEditor 
{

	public VJMidiNoteDataSourceEditor()
    {
    }

    public override void OnInspectorGUI()
    {
		VJMidiNoteDataSource src = target as VJMidiNoteDataSource;

        base.OnInspectorGUI();

		Rect r = GUILayoutUtility.GetLastRect();
		r.y -= 6;

		float lowerNote = (float)src.lowerNote;
		float upperNote = (float)src.upperNote;
		EditorGUI.MinMaxSlider(new GUIContent("Range[" +src.lowerNote +":"+ src.upperNote + "]"),  r, ref lowerNote, ref upperNote, 0,  127 ); 
		src.lowerNote = (int)lowerNote;
		src.upperNote = (int)upperNote;
		
		if (GUI.changed) {
			EditorUtility.SetDirty(target);
		}        
	}
}
