using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(VJGameObjectModifier))]
[CanEditMultipleObjects]
public class VJGameObjectModifierEditor : VJBaseModifierEditor 
{
    public VJGameObjectModifierEditor()
    {
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
