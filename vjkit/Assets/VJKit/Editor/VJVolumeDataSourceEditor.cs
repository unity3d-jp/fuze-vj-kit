using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(VJVolumeDataSource))]
public class VJVolumeDataSourceEditor : VJAbstractDataSourceEditor 
{

    public VJVolumeDataSourceEditor()
    {
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
