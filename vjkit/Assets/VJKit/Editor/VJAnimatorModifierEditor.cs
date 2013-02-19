using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(VJAnimatorModifier))]
public class VJAnimatorModifierEditor : VJBaseModifierEditor 
{
    public VJAnimatorModifierEditor()
    {
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
