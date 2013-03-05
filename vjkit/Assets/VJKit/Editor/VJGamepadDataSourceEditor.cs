using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(VJGamepadDataSource))]
public class VJGamepadDataSourceEditor : VJAbstractDataSourceEditor 
{
    public VJGamepadDataSourceEditor()
    {
    }

    public override void OnInspectorGUI()
    {
		VJGamepadDataSource src = target as VJGamepadDataSource;

		if( src.sourceName == null || src.sourceName.Length == 0 ) {
        	src.sourceName = VJGamePadButtonUtility.GetButtonNameOf(src.button);
            EditorUtility.SetDirty(target);
		}

        base.OnInspectorGUI();
                
        if (GUI.changed) {
        	src.sourceName = VJGamePadButtonUtility.GetButtonNameOf(src.button);
            EditorUtility.SetDirty(target);
        }        
    }
}
