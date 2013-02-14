using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(VJGameObjectTrigger))]
public class VJGameObjectTriggerEditor : VJBaseTriggerEditor 
{
    public VJGameObjectTriggerEditor()
    {
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
