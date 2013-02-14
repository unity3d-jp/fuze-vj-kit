using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(VJMessageTrigger))]
public class VJMessageTriggerEditor : VJBaseTriggerEditor 
{
    public VJMessageTriggerEditor()
    {
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
