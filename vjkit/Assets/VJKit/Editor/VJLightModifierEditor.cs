using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(VJLightModifier))]
public class VJLightModifierEditor : VJBaseModifierEditor 
{
    public VJLightModifierEditor()
    {
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
