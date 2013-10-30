using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(VJMidiKnobDataSource))]
public class VJMidiKnobDataSourceEditor : VJAbstractDataSourceEditor 
{

	public VJMidiKnobDataSourceEditor()
    {
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
