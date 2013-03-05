using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(VJOnOffTrigger))]
public class VJOnOffTriggerEditor : VJBaseTriggerEditor 
{
    public VJOnOffTriggerEditor()
    {
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
