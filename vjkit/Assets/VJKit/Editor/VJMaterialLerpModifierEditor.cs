using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(VJMaterialLerpModifier))]
public class VJMaterialLerpModifierEditor : VJBaseModifierEditor 
{
    public VJMaterialLerpModifierEditor()
    {
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

		VJMaterialLerpModifier ml = target as VJMaterialLerpModifier;
			
		Material[] mats = ml.renderer.sharedMaterials;

        string[] matNames = new string[mats.Length];
        int index = 0;

        for (int i = 0; i < mats.Length; i++)
        {
            matNames[i] = mats[i].name;
        }

        EditorGUILayout.BeginHorizontal();

        index = EditorGUILayout.Popup("Material to Lerp", ml.matIndex, matNames);
		ml.matIndex = index;

        EditorGUILayout.EndHorizontal();
        
        if (GUI.changed) {
            EditorUtility.SetDirty(target);
        }        
    }
}
