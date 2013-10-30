using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(VJMidiNotePitchDataSource))]
public class VJMidiNotePitchDataSourceEditor : VJAbstractDataSourceEditor 
{

	public VJMidiNotePitchDataSourceEditor()
    {
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
	}
}
