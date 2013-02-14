using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(VJMaterialPropertyModifier))]
public class VJMaterialPropertyModifierEditor : VJBaseModifierEditor 
{
    public VJMaterialPropertyModifierEditor()
    {
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
